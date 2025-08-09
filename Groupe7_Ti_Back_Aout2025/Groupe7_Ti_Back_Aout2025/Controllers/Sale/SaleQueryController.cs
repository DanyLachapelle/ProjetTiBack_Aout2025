using Application.Sales.query;
using Application.Sales.query.GetSalesByDate;
using Application.Sales.query.GetSalesById;
using Infrastructure.User.Sale;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Application.Sales.query.GetAllSales;


namespace Groupe7_Ti_Back_Aout2025.Controllers.Sale;

[ApiController]
[Route("api/[controller]")]
public class SaleQueryController: ControllerBase
{
    private readonly ISaleRepository _saleRepository;
    private readonly SalesQueryProcessor _salesQueryProcessor;
    
    public SaleQueryController(SalesQueryProcessor salesQueryProcessor, ISaleRepository saleRepository)
    {
        _salesQueryProcessor = salesQueryProcessor;
        _saleRepository = saleRepository;
    }
    
    [HttpGet("GetSaleById/{id}")]
    public IActionResult GetSaleById(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new { message = "ID de vente invalide." });
        }

        try
        {
            var query = new GetSalesByIdQuery(id);
            var sale = _salesQueryProcessor.GetSaleById(query);

            if (sale == null)
            {
                return NotFound(new { message = "Vente non trouvée." });
            }
            return Ok(sale);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erreur lors de la récupération de la vente.", error = ex.Message });
        }
    }
    
    [HttpGet("GetSalesByDate")]
    public IActionResult GetSalesByDate([FromQuery] DateTime date)
    {
        if (date == default)
        {
            return BadRequest(new { message = "Date invalide." });
        }

        try
        {
            // Toujours inclure les items dans la requête
            var query = new GetSalesByDateQuery(date, includeItems: true);
            var sales = _salesQueryProcessor.GetSalesByDate(query);

            if (sales == null || !sales.Sales.Any())
            {
                return NotFound(new { message = "Aucune vente trouvée pour cette date." });
            }
            return Ok(sales);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erreur lors de la récupération des ventes.", error = ex.Message });
        }
    }
    
    [HttpGet("GetAllSales")]
    public IActionResult GetAllSales()
    {
        try
        {
            var query = new GetAllSalesQuery();
            var sales = _salesQueryProcessor.GetAllSales(query);

            if (sales == null || !sales.Sales.Any())
            {
                return Ok(new { Sales = new List<object>(), message = "Aucune vente trouvée." });
            }
            return Ok(sales);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erreur lors de la récupération des ventes.", error = ex.Message, stackTrace = ex.StackTrace });
        }
    }




}