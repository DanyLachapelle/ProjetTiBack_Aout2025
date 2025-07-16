using Application.Mocktails.commands.createMocktail;
using Application.Mocktails.commands.deleteMocktail;
using Application.Mocktails.commands.updateMocktail;
using Application.Utils;

namespace Application.Mocktails.commands;

public class MocktailCommandProcessor
{
    private readonly ICommandHandler<DeleteMocktailCommand, DeleteMocktailOutput> _deleteMocktailHandler;
    private readonly ICommandHandler<CreateMocktailCommand, CreateMocktailOutput> _createMocktailHandler;
    private readonly ICommandHandler<UpdateMocktailCommand, UpdateMocktailOutput> _updateMocktailHandler;
    
public MocktailCommandProcessor(
        ICommandHandler<DeleteMocktailCommand, DeleteMocktailOutput> deleteMocktailHandler,
        ICommandHandler<CreateMocktailCommand, CreateMocktailOutput> createMocktailHandler,
        ICommandHandler<UpdateMocktailCommand, UpdateMocktailOutput> updateMocktailHandler)
    {
        _deleteMocktailHandler = deleteMocktailHandler;
        _createMocktailHandler = createMocktailHandler;
        _updateMocktailHandler = updateMocktailHandler;
    }
    
public CreateMocktailOutput CreateMocktail(CreateMocktailCommand command)
    {
        return _createMocktailHandler.Handle(command);
    }


    public DeleteMocktailOutput DeleteMocktail(DeleteMocktailCommand command)
    {
        return _deleteMocktailHandler.Handle(command);
    }
    
    public UpdateMocktailOutput UpdateMocktail(UpdateMocktailCommand command)
    {
        return _updateMocktailHandler.Handle(command);
    }
    
}