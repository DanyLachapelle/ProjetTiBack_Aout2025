using System;
using System.Collections.Generic;
using Application.Utils;
using Infrastructure.Mocktail;

namespace Application.Mocktails.commands.deleteMocktail;

public class DeleteMocktailHandler:ICommandHandler<DeleteMocktailCommand, DeleteMocktailOutput>
{
    private readonly IMocktailRepository _mocktailRepository;
    
    public DeleteMocktailHandler(IMocktailRepository mocktailRepository)
    {
        _mocktailRepository = mocktailRepository;
    }
    public DeleteMocktailOutput Handle(DeleteMocktailCommand command)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command), "Command cannot be null");

        if (command.Id <= 0)
            throw new ArgumentException("Invalid mocktail ID provided", nameof(command.Id));

        var mocktail = _mocktailRepository.GetMocktailById(command.Id);
        if (mocktail == null)
            throw new KeyNotFoundException($"Mocktail with ID {command.Id} not found");

        _mocktailRepository.DeleteMocktail(mocktail);

        return new DeleteMocktailOutput();
    }
}