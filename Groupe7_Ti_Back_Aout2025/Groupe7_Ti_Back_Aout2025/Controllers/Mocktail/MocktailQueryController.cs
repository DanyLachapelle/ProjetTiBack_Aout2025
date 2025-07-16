using Application.DTOs;
using Application.Mocktails.query;
using Application.Mocktails.query.getbyidMocktail;
using Application.Mocktails.Query.GetByIdMocktail;
using Application.Services;
using Infrastructure.Mocktail;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Groupe7_Ti_Back_Aout2025.Controllers.Mocktail;
[ApiController]
[Route("api/[controller]")]
public class MocktailQueryController:ControllerBase
{
    private readonly MocktailQueryProcessor _mocktailQueryProcessor;
    private readonly IMocktailRepository _mocktailRepository;
    
    public MocktailQueryController(MocktailQueryProcessor mocktailQueryProcessor, IMocktailRepository mocktailRepository)
    {
        _mocktailQueryProcessor = mocktailQueryProcessor;
        _mocktailRepository = mocktailRepository;
    }
    
    [HttpGet("{id}")]
    public ActionResult<MocktailDto?> GetMocktailById(int id)
    {
        var query = new getbyidMocktailQuery(id);
        var result = _mocktailQueryProcessor.GetMocktailById(query);
        
        if (result == null)
        {
            return NotFound();
        }
        
        return Ok(result);
    }


    

    
    
    
    
    

    
    
}