using Application.Mocktails.commands.deleteMocktail;
using Application.Utils;

namespace Application.Mocktails.commands;

public class MocktailCommandProcessor
{
    private readonly ICommandHandler<DeleteMocktailCommand, DeleteMocktailOutput> _deleteMocktailHandler;
    
    public MocktailCommandProcessor(ICommandHandler<DeleteMocktailCommand, DeleteMocktailOutput> deleteMocktailHandler)
    {
        _deleteMocktailHandler = deleteMocktailHandler;
    }
    
    public DeleteMocktailOutput DeleteMocktail(DeleteMocktailCommand command)
    {
        return _deleteMocktailHandler.Handle(command);
    }
}