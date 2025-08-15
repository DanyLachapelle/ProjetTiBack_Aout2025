using System;
using Application.Mocktails.commands;
using Application.Mocktails.commands.createMocktail;
using Application.Mocktails.commands.deleteMocktail;
using Application.Mocktails.commands.updateMocktail;
using Infrastructure.Mocktail;
using Microsoft.AspNetCore.Mvc;

namespace Groupe7_Ti_Back_Aout2025.Controllers.Mocktail;
[ApiController]
[Route("api/[controller]")]
public class MocktailCommandController:ControllerBase
{
    private readonly MocktailCommandProcessor _mocktailCommandProcessor;
    private readonly IMocktailRepository _mocktailRepository;
    
    public MocktailCommandController(MocktailCommandProcessor mocktailCommandProcessor, IMocktailRepository mocktailRepository)
    {
        _mocktailCommandProcessor = mocktailCommandProcessor;
        _mocktailRepository = mocktailRepository;
    }
    
    [HttpDelete("DeleteMocktail/{id}")]
    public IActionResult Delete(int id)
    {
        var command = new DeleteMocktailCommand(id);
        var result = _mocktailCommandProcessor.DeleteMocktail(command);
        
        if (result == null)
        {
            return NotFound();
        }
        
        return Ok(new DeleteMocktailOutput());
    }
    
    [HttpPost("CreateMocktail")]
    public IActionResult Create([FromBody] CreateMocktailCommand command)
    {
        if (command == null)
        {
            return BadRequest(new { message = "Commande invalide." });
        }

        try
        {
            var result = _mocktailCommandProcessor.CreateMocktail(command);
            // On retourne le mocktail créé sans exiger un id en entrée
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erreur lors de la création du mocktail.", error = ex.Message });
        }
    }

    [HttpPut("UpdateMocktail/{id}")]
    public IActionResult Update(int id, [FromBody] UpdateMocktailCommand command)
    {
        try
        {
            command.Id = id; // injecte l'id du route dans la commande
            var result = _mocktailCommandProcessor.UpdateMocktail(command);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erreur interne du serveur", error = ex.Message });
        }
    }






}