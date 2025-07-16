using Application.Mocktails.commands;
using Application.Mocktails.commands.deleteMocktail;
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
    
    [HttpDelete("{id}")]
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


}