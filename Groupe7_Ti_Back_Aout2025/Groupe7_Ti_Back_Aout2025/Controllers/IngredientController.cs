// using Application.Services;
// using Microsoft.AspNetCore.Mvc;
//
// namespace Groupe7_Ti_Back_Aout2025.Controllers;
//
// [ApiController]
// [Route("api/[controller]")]
// public class IngredientController : ControllerBase
// {
//     private readonly IMocktailService _mocktailService;
//
//     public IngredientController(IMocktailService mocktailService)
//     {
//         _mocktailService = mocktailService;
//     }
//
//     [HttpGet]
//     public async Task<IActionResult> GetAll()
//     {
//         try
//         {
//             var ingredients = await _mocktailService.GetAllIngredientsAsync();
//             return Ok(ingredients);
//         }
//         catch (Exception ex)
//         {
//             return StatusCode(500, new { message = "Erreur interne du serveur", error = ex.Message });
//         }
//     }
// } 