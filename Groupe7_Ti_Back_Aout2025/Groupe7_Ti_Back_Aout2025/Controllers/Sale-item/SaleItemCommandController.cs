using Application.SalesItem.commands;
using Application.SalesItem.commands.AddItemToSale;
using Application.SalesItem.commands.RemoveItemFromSale;
using Application.SalesItem.commands.UpdateSaleItem;
using Infrastructure.Sale;
using Microsoft.AspNetCore.Mvc;

namespace Groupe7_Ti_Back_Aout2025.Controllers.Sale_item;

[ApiController]
[Route("api/[controller]")]
public class SaleItemCommandController:ControllerBase
{
    private readonly ISaleItemRepository _saleItemRepository;
    private readonly SaleItemCommandProcessor _saleItemCommandProcessor;
    
    public SaleItemCommandController(SaleItemCommandProcessor saleItemCommandProcessor, ISaleItemRepository saleItemRepository)
    {
        _saleItemCommandProcessor = saleItemCommandProcessor;
        _saleItemRepository = saleItemRepository;
    }
    
    [HttpPost("AddSaleItem")]
    public IActionResult AddSaleItem([FromBody] AddItemToSaleCommand command)
    {
        if (command == null || command.SaleId <= 0 || command.MocktailId <= 0 || command.Quantity <= 0)
        {
            return BadRequest(new { message = "Données de l'article de vente invalides." });
        }

        try
        {
            var result = _saleItemCommandProcessor.AddItemToSale(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erreur lors de l'ajout de l'article de vente.", error = ex.Message });
        }
    }
    
    [HttpPut("UpdateSaleItem")]
    public IActionResult UpdateSaleItem([FromBody] UpdateSaleItemCommand command)
    {
        if (command == null || command.ItemId <= 0 || command.NewQuantity <= 0)
        {
            return BadRequest(new { message = "Données de l'article de vente invalides." });
        }

        try
        {
            var result = _saleItemCommandProcessor.UpdateSaleItem(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new 
            { 
                message = "Erreur lors de la mise à jour de l'article de vente.", 
                error = ex.Message, 
                stackTrace = ex.StackTrace 
            });
        }
    }


    [HttpDelete("RemoveSaleItem/{saleId}/{itemId}")]
    public IActionResult RemoveSaleItem(int saleId, int itemId)
    {
        if (saleId <= 0 || itemId <= 0)
        {
            return BadRequest(new { message = "ID de l'article de vente invalide." });
        }

        try
        {
            var command = new RemoveItemFromSaleCommand(saleId, itemId);
            var result = _saleItemCommandProcessor.RemoveItemFromSale(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new 
            { 
                message = "Erreur lors de la suppression de l'article de vente.", 
                error = ex.Message, 
                stackTrace = ex.StackTrace 
            });
           
        }
    }
}