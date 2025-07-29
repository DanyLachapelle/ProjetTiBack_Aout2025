using Application.Sales.query;
using Application.Sales.query.GetSalesById;
using Infrastructure.User.Sale;
using Microsoft.AspNetCore.Mvc;

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


}