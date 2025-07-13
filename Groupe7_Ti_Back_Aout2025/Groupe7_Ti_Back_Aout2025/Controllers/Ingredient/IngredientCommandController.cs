using Application.Ingredient.commands;
using Application.Ingredient.commands.createIngredient;
using Infrastructure.User.Ingredient;
using Microsoft.AspNetCore.Mvc;

namespace Groupe7_Ti_Back_Aout2025.Controllers.Ingredient;

[ApiController]
[Route("api/ingredients")]
public class IngredientCommandController:ControllerBase
{
    private readonly IngredientCommandProcessor _ingredientCommandsProcessor;
    private readonly IIngredientRepository _ingredientRepository;
    
    public IngredientCommandController(IngredientCommandProcessor ingredientCommandsProcessor, IIngredientRepository ingredientRepository)
    {
        _ingredientCommandsProcessor = ingredientCommandsProcessor;
        _ingredientRepository = ingredientRepository;
    }
    
    [HttpPost("createIngredient")]
    [ProducesResponseType(typeof(CreateIngredientOutput), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
    public ActionResult<CreateIngredientOutput> CreateIngredient([FromBody] CreateIngredientQuery command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = _ingredientCommandsProcessor.CreateIngredient(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            // Log the exception if necessary
            return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
        }
    }
}