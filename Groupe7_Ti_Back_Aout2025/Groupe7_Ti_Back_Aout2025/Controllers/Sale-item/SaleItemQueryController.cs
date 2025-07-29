using Application.SalesItem.query;
using Application.SalesItem.query.GetAllItemBySale;
using Infrastructure.Sale;
using Microsoft.AspNetCore.Mvc;

namespace Groupe7_Ti_Back_Aout2025.Controllers.Sale_item;

[ApiController]
[Route("api/[controller]")]
public class SaleItemQueryController:ControllerBase
{
    private readonly ISaleItemRepository _saleItemRepository;
    private readonly SaleItemQueryProcessor _saleItemQueryProcessor;
    
    public SaleItemQueryController(SaleItemQueryProcessor saleItemQueryProcessor, ISaleItemRepository saleItemRepository)
    {
        _saleItemQueryProcessor = saleItemQueryProcessor;
        _saleItemRepository = saleItemRepository;
    }
    
    [HttpGet("GetSaleItemById/{SaleId}")]
    public IActionResult GetSaleItemById(int SaleId)
    {
        if (SaleId <= 0)
        {
            return BadRequest(new { message = "ID de l'article de vente invalide." });
        }

        try
        {
            var query = new GetAllItemsBySaleQuery(SaleId);
            var saleItem = _saleItemQueryProcessor.GetAllItemsBySale(query);

            if (saleItem == null)
            {
                return NotFound(new { message = "Article de vente non trouvé." });
            }
            return Ok(saleItem);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erreur lors de la récupération de l'article de vente.", error = ex.Message });
        }
    }
}