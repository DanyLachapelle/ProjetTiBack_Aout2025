using System;
using Application.Ingredient.commands;
using Application.Ingredient.commands.createIngredient;
using Application.Ingredient.commands.deleteIngredient;
using Application.Ingredient.commands.UpdateLimitIngredient;
using Application.Ingredient.commands.UpdateQuantityIngredient;
using Infrastructure.User.Ingredient;
using Microsoft.AspNetCore.Http;
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
    
    [HttpDelete("deleteIngredient/{id}")]
    [ProducesResponseType(typeof(DeleteIngredientOutput), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
    public ActionResult<DeleteIngredientOutput> DeleteIngredient(int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = _ingredientCommandsProcessor.DeleteIngredient(id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
        }
    }
    
    [HttpPut("updateLimitIngredient/{id}")]
    [ProducesResponseType(typeof(UpdateLimitIngredientOutput), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public ActionResult<UpdateLimitIngredientOutput> UpdateLimitIngredient(
        int id,
        [FromBody] UpdateLimitIngredientQuery command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = _ingredientCommandsProcessor.UpdateLimitIngredient(id, command);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    
    [HttpPut("updateQuantity/{id}")]
    public ActionResult<UpdateQuantityIngredientOutput> UpdateQuantity(int id, [FromBody] decimal amount)
    {
        try
        {
            var command = new UpdateQuantityIngredientQuery() { Id = id, Amount = amount };
            var result = _ingredientCommandsProcessor.UpdateQuantityIngredient(id, command);

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
        }
    }



    

    
}