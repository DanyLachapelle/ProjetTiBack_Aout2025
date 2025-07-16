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

        if (command.id <= 0)
            throw new ArgumentException("Invalid mocktail ID provided", nameof(command.id));

        var mocktail = _mocktailRepository.GetMocktailById(command.id);
        if (mocktail == null)
            throw new KeyNotFoundException($"Mocktail with ID {command.id} not found");

        _mocktailRepository.DeleteMocktail(mocktail);

        return new DeleteMocktailOutput();
    }
}