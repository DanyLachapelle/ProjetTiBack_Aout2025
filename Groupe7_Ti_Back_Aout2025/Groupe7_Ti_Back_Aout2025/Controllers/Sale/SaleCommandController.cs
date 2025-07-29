using Application.Sales.commands;
using Application.Sales.commands.CreateSale;
using Application.Sales.commands.DeleteSale;
using Application.Sales.commands.UpdateSale;
using Infrastructure.User.Sale;
using Microsoft.AspNetCore.Mvc;

namespace Groupe7_Ti_Back_Aout2025.Controllers.Sale;

[ApiController]
[Route("api/[controller]")]
public class SaleCommandController: ControllerBase
{
    private readonly ISaleRepository _saleRepository;
    private readonly SaleCommandProcessor _saleCommandProcessor;
    
    public SaleCommandController(SaleCommandProcessor saleCommandProcessor, ISaleRepository saleRepository)
    {
        _saleCommandProcessor = saleCommandProcessor;
        _saleRepository = saleRepository;
    }
    
    [HttpPost("CreateSale")]
    public IActionResult Create([FromBody] CreateSaleCommand command)
    {
        if (command == null)
        {
            return BadRequest(new { message = "Commande invalide." });
        }

        try
        {
            var result = _saleCommandProcessor.CreateSale(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erreur lors de la création de la vente.", error = ex.Message });
        }
    }
    
    [HttpPut("UpdateSale")]
    public IActionResult Update([FromBody] UpdateSaleCommand command)
    {
        if (command == null)
        {
            return BadRequest(new { message = "Commande invalide." });
        }

        try
        {
            var result = _saleCommandProcessor.UpdateSale(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erreur lors de la mise à jour de la vente.", error = ex.Message });
        }
    }
    
    [HttpDelete("DeleteSale/{id}")]
    public IActionResult Delete(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new { message = "ID de vente invalide." });
        }

        try
        {
            var command = new DeleteSaleCommand { SaleId = id };
            var result = _saleCommandProcessor.DeleteSale(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erreur lors de la suppression de la vente.", error = ex.Message });
        }
    }
}