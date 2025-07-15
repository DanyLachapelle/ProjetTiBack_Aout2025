using System;
using System.Threading.Tasks;
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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMocktailRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Données invalides", errors = ModelState });
            }

            var createdMocktail = await _mocktailService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = createdMocktail.Id }, createdMocktail);
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

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMocktailRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Données invalides", errors = ModelState });
            }

            var updatedMocktail = await _mocktailService.UpdateAsync(id, request);
            return Ok(updatedMocktail);
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