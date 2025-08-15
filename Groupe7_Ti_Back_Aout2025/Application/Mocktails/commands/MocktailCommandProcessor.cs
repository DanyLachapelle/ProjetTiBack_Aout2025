using Application.Mocktails.commands.createMocktail;
using Application.Mocktails.commands.deleteMocktail;
using Application.Mocktails.commands.updateMocktail;
using Application.Utils;

namespace Application.Mocktails.commands;

// Façade centrale pour les commandes relatives aux mocktails
public class MocktailCommandProcessor
{
    // Handlers pour les différentes opérations
    private readonly ICommandHandler<DeleteMocktailCommand, DeleteMocktailOutput> _deleteMocktailHandler;
    private readonly ICommandHandler<CreateMocktailCommand, CreateMocktailOutput> _createMocktailHandler;
    private readonly ICommandHandler<UpdateMocktailCommand, UpdateMocktailOutput> _updateMocktailHandler;
    
    // Injection des dépendances
    public MocktailCommandProcessor(
        ICommandHandler<DeleteMocktailCommand, DeleteMocktailOutput> deleteMocktailHandler,
        ICommandHandler<CreateMocktailCommand, CreateMocktailOutput> createMocktailHandler,
        ICommandHandler<UpdateMocktailCommand, UpdateMocktailOutput> updateMocktailHandler)
    {
        _deleteMocktailHandler = deleteMocktailHandler;
        _createMocktailHandler = createMocktailHandler;
        _updateMocktailHandler = updateMocktailHandler;
    }
    
    // Création d'un nouveau mocktail
    public CreateMocktailOutput CreateMocktail(CreateMocktailCommand command)
    {
        return _createMocktailHandler.Handle(command);
    }

    // Suppression d'un mocktail existant
    public DeleteMocktailOutput DeleteMocktail(DeleteMocktailCommand command)
    {
        return _deleteMocktailHandler.Handle(command);
    }
    
    // Mise à jour d'un mocktail existant
    public UpdateMocktailOutput UpdateMocktail(UpdateMocktailCommand command)
    {
        return _updateMocktailHandler.Handle(command);
    }
}