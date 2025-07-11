using Application.Services;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Groupe7_Ti_Back_Aout2025.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MocktailController : ControllerBase
{
    private readonly IMocktailService _mocktailService;

    public MocktailController(IMocktailService mocktailService)
    {
        _mocktailService = mocktailService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var mocktails = await _mocktailService.GetAllAsync();
            return Ok(mocktails);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erreur interne du serveur", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var mocktail = await _mocktailService.GetByIdAsync(id);
            if (mocktail == null)
            {
                return NotFound(new { message = "Mocktail non trouvé" });
            }
            return Ok(mocktail);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erreur interne du serveur", error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _mocktailService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erreur interne du serveur", error = ex.Message });
        }
    }
} 