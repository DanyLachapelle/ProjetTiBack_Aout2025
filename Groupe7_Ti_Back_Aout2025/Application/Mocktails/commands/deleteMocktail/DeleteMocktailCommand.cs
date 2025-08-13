namespace Application.Mocktails.commands.deleteMocktail;

public class DeleteMocktailCommand
{
    public int Id { get; set; }
    
    public DeleteMocktailCommand(int id)
    {
        this.Id = id;
    }
}