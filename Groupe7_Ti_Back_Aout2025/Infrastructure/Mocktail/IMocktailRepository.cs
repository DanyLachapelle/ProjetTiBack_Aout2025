using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace Infrastructure.Mocktail;

public interface IMocktailRepository
{
    List<mocktail> GetAllMocktail();
    void CreateMocktail(mocktail mocktail);
    void DeleteMocktail(mocktail mocktail);
    mocktail GetMocktailById(int commandId);
    void UpdateMocktail(mocktail mocktail);
} 