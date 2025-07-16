using Application.Services;
using Application.DTOs;
using Infrastructure.Mocktail;
using Microsoft.AspNetCore.Mvc;

namespace Groupe7_Ti_Back_Aout2025.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MocktailController : ControllerBase
{
    private readonly IMocktailService _mocktailService;
    private readonly IMocktailRepository _mocktailRepository;
    
    public MocktailController(IMocktailService mocktailService, IMocktailRepository mocktailRepository)
    {
        _mocktailService = mocktailService ?? throw new ArgumentNullException(nameof(mocktailService));
        _mocktailRepository = mocktailRepository ?? throw new ArgumentNullException(nameof(mocktailRepository));
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

    // [HttpGet("{id}")]
    // public async Task<IActionResult> GetById(int id)
    // {
    //     try
    //     {
    //         var mocktail = await _mocktailService.GetMocktailById(id);
    //         if (mocktail == null)
    //         {
    //             return NotFound(new { message = "Mocktail non trouvé" });
    //         }
    //         return Ok(mocktail);
    //     }
    //     catch (Exception ex)
    //     {
    //         return StatusCode(500, new { message = "Erreur interne du serveur", error = ex.Message });
    //     }
    // }
    
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        try
        {
            var mocktail = _mocktailRepository.GetMocktailById(id);
            if (mocktail == null)
                return NotFound(new { message = "Mocktail non trouvé" });

            var dto = new MocktailDto
            {
                Id = mocktail.id,
                Name = mocktail.nom,
                Description = mocktail.description,
                Price = mocktail.prix,
                Available = IsAvailable(mocktail),  // Ta méthode dispo côté contrôleur
                Image = mocktail.image,
                Ingredients = mocktail.MocktailIngredients.Select(mi => new MocktailIngredientDto
                {
                    Name = mi.Ingredient.name,
                    Quantity = mi.quantite,
                    Unit = mi.unite
                }).ToList()
            };

            return Ok(dto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erreur interne du serveur", error = ex.Message });
        }
    }

    private bool IsAvailable(Domain.mocktail mocktail)
    {
        return mocktail.MocktailIngredients.All(mi => 
            mi.Ingredient.quantity >= mi.quantite
        );
    }
    // [HttpPost]
    // public async Task<IActionResult> Create([FromBody] CreateMocktailRequest request)
    // {
    //     try
    //     {
    //         if (!ModelState.IsValid)
    //         {
    //             return BadRequest(new { message = "Données invalides", errors = ModelState });
    //         }
    //
    //         var createdMocktail = await _mocktailService.CreateAsync(request);
    //         return CreatedAtAction(nameof(GetById), new { id = createdMocktail.Id }, createdMocktail);
    //     }
    //     catch (ArgumentException ex)
    //     {
    //         return BadRequest(new { message = ex.Message });
    //     }
    //     catch (Exception ex)
    //     {
    //         return StatusCode(500, new { message = "Erreur interne du serveur", error = ex.Message });
    //     }
    // }

    // [HttpPut("{id}")]
    // public IActionResult Update(int id, [FromBody] UpdateMocktailRequest request)
    // {
    //     try
    //     {
    //         if (!ModelState.IsValid)
    //         {
    //             return BadRequest(new { message = "Données invalides", errors = ModelState });
    //         }
    //
    //         var updatedMocktail = _mocktailService.Update(id, request); // méthode synchrone
    //         return Ok(updatedMocktail);
    //     }
    //     catch (ArgumentException ex)
    //     {
    //         return BadRequest(new { message = ex.Message });
    //     }
    //     catch (Exception ex)
    //     {
    //         return StatusCode(500, new { message = "Erreur interne du serveur", error = ex.Message });
    //     }
    // }


    // [HttpDelete("{id}")]
    // public async Task<IActionResult> Delete(int id)
    // {
    //     try
    //     {
    //         await _mocktailService.DeleteMocktail(id);
    //         return NoContent();
    //     }
    //     catch (Exception ex)
    //     {
    //         return StatusCode(500, new { message = "Erreur interne du serveur", error = ex.Message });
    //     }
    // }
    
    // [HttpDelete("{id}")]
    // public IActionResult Delete(int id)
    // {
    //     try
    //     {
    //         _mocktailService.DeleteMocktail(id); // appel synchrone
    //         return NoContent();
    //     }
    //     catch (Exception ex)
    //     {
    //         return StatusCode(500, new { message = "Erreur interne du serveur", error = ex.Message });
    //     }
    // }

} 