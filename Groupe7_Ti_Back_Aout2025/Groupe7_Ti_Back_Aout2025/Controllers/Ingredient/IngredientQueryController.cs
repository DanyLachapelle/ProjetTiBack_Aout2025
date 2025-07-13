using Application.Ingredient.query;
using Application.Ingredient.query.getAllIngredient;
using Infrastructure.User.Ingredient;
using Microsoft.AspNetCore.Mvc;

namespace Groupe7_Ti_Back_Aout2025.Controllers.Ingredient;

// [ApiController]
[ApiController]
[Route("api/ingredients")]
public class IngredientQueryController:ControllerBase
{
    private readonly IngredientGetAllQueryProcessor _ingredientqueryCommandsProcessor;
    private readonly IIngredientRepository _ingredientRepository;
    
    public IngredientQueryController(IngredientGetAllQueryProcessor ingredientqueryCommandsProcessor, IIngredientRepository ingredientRepository)
    {
        _ingredientqueryCommandsProcessor = ingredientqueryCommandsProcessor;
        _ingredientRepository = ingredientRepository;
    }
    
    [HttpGet("getAllIngredients")]
    [ProducesResponseType(typeof(IngredientGetAllOutput), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
    public ActionResult<IngredientGetAllOutput> GetAllIngredients([FromQuery] IngredientGetAllQuery query)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = _ingredientqueryCommandsProcessor.GetAllIngredients(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            // Log the exception if necessary
            return StatusCode(500, new { message = "An unexpected error occurred." });
        }
    }
}