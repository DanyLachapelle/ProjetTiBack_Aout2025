namespace Application.Mocktails.commands.deleteMocktail;

public class DeleteMocktailCommand
{
    public int id { get; set; }
    
    public DeleteMocktailCommand(int id)
    {
        this.id = id;
    }
}