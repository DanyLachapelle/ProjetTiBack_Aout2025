using Application.SalesItem.query;
using Application.SalesItem.query.GetAllItemBySale;
using Application.SalesItem.query.GetItemBySaleById;
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
    public IActionResult GetSaleItemById(int SaleId, int ItemId)
    {
        if (SaleId <= 0 || ItemId <= 0)
        {
            return BadRequest(new { message = "Invalid sale item ID." });
        }

        try
        {
            var query = new GetItemBySaleByIdQuery(SaleId, ItemId);
            var saleItem = _saleItemQueryProcessor.GetItemBySaleId(query);

            if (saleItem == null)
            {
                return NotFound(new { message = "Sale item not found." });
            }
            return Ok(saleItem);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving the sale item.", error = ex.Message });
        }
    }
     
     
     
     
    
    [HttpGet("GetSaleItemBySale/{SaleId}")]
    public IActionResult GetSaleItemBySale(int SaleId)
    {
        if (SaleId <= 0)
        {
            return BadRequest(new { message = "Invalid sale item ID." });
        }

        try
        {
            var query = new GetAllItemsBySaleQuery(SaleId);
            var saleItem = _saleItemQueryProcessor.GetAllItemsBySale(query);

            if (saleItem == null)
            {
                return NotFound(new { message = "Sale item not found." });
            }
            return Ok(saleItem);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving the sale item.", error = ex.Message });
        }
    }
    
    
}