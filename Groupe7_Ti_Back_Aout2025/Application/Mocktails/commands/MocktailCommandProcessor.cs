using Application.Mocktails.commands.createMocktail;
using Application.Mocktails.commands.deleteMocktail;
using Application.Utils;

namespace Application.Mocktails.commands;

public class MocktailCommandProcessor
{
    private readonly ICommandHandler<DeleteMocktailCommand, DeleteMocktailOutput> _deleteMocktailHandler;
    private readonly ICommandHandler<CreateMocktailCommand, CreateMocktailOutput> _createMocktailHandler;
    
    public MocktailCommandProcessor(
        ICommandHandler<DeleteMocktailCommand, DeleteMocktailOutput> deleteMocktailHandler,
        ICommandHandler<CreateMocktailCommand, CreateMocktailOutput> createMocktailHandler)
    {
        _deleteMocktailHandler = deleteMocktailHandler;
        _createMocktailHandler = createMocktailHandler;
    }
    
    public DeleteMocktailOutput DeleteMocktail(DeleteMocktailCommand command)
    {
        return _deleteMocktailHandler.Handle(command);
    }
    
    public CreateMocktailOutput CreateMocktail(CreateMocktailCommand command)
    {
        return _createMocktailHandler.Handle(command);
    }
}