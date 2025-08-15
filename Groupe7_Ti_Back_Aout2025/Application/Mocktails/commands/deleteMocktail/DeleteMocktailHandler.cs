using System;
using System.Collections.Generic;
using Application.Utils;
using Infrastructure.Mocktail;

namespace Application.Mocktails.commands.deleteMocktail;

// Handler for deleting a mocktail
public class DeleteMocktailHandler : ICommandHandler<DeleteMocktailCommand, DeleteMocktailOutput>
{
    // Repository for mocktail data access
    private readonly IMocktailRepository _mocktailRepository;
    
    // Repository dependency injection
    public DeleteMocktailHandler(IMocktailRepository mocktailRepository)
    {
        _mocktailRepository = mocktailRepository;
    }

    // Main command processing method
    public DeleteMocktailOutput Handle(DeleteMocktailCommand command)
    {
        // Command validation
        if (command == null)
            throw new ArgumentNullException(nameof(command), "Command cannot be null");

        // ID validation
        if (command.Id <= 0)
            throw new ArgumentException("Invalid mocktail ID provided", nameof(command.Id));

        // Retrieving the mocktail
        var mocktail = _mocktailRepository.GetMocktailById(command.Id);
        if (mocktail == null)
            throw new KeyNotFoundException($"Mocktail with ID {command.Id} not found");

        // Actual deletion
        _mocktailRepository.DeleteMocktail(mocktail);

        // Returning empty output (useful pattern for deletion confirmations)
        return new DeleteMocktailOutput();
    }
}