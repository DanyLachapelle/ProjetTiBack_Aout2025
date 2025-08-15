using System;
using System.Collections.Generic;
using Application.Utils;
using Infrastructure.Mocktail;

namespace Application.Mocktails.commands.deleteMocktail;

// Handler pour la suppression d'un mocktail
public class DeleteMocktailHandler : ICommandHandler<DeleteMocktailCommand, DeleteMocktailOutput>
{
    // Répository pour l'accès aux données des mocktails
    private readonly IMocktailRepository _mocktailRepository;
    
    // Injection de dépendance du repository
    public DeleteMocktailHandler(IMocktailRepository mocktailRepository)
    {
        _mocktailRepository = mocktailRepository;
    }

    // Méthode principale de traitement de la commande
    public DeleteMocktailOutput Handle(DeleteMocktailCommand command)
    {
        // Validation de la commande
        if (command == null)
            throw new ArgumentNullException(nameof(command), "Command cannot be null");

        // Validation de l'ID
        if (command.Id <= 0)
            throw new ArgumentException("Invalid mocktail ID provided", nameof(command.Id));

        // Récupération du mocktail
        var mocktail = _mocktailRepository.GetMocktailById(command.Id);
        if (mocktail == null)
            throw new KeyNotFoundException($"Mocktail with ID {command.Id} not found");

        // Suppression effective
        _mocktailRepository.DeleteMocktail(mocktail);

        // Retour d'une output vide (pattern utile pour les confirmations de suppression)
        return new DeleteMocktailOutput();
    }
}