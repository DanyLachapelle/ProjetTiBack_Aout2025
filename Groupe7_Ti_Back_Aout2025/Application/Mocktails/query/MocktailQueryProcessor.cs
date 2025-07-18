using System.Collections.Generic;
using Application.DTOs;
using Application.Mocktails.query.getAllMocktail;
using Application.Mocktails.query.getbyidMocktail;
using Application.Utils;

namespace Application.Mocktails.query;

public class MocktailQueryProcessor
{
    private readonly IQueryHandler<GetbyidMocktailQuery, MocktailDto> _getByIdMocktailHandler;
    private readonly IQueryHandler<GetAllMocktailQuery, List<MocktailDto>> _getAllMocktailsHandler;
    
    public MocktailQueryProcessor(
        IQueryHandler<GetbyidMocktailQuery, MocktailDto> getByIdMocktailHandler,
        IQueryHandler<GetAllMocktailQuery, List<MocktailDto>> getAllMocktailsHandler)
    {
        _getByIdMocktailHandler = getByIdMocktailHandler;
        _getAllMocktailsHandler = getAllMocktailsHandler;
    }
    
    public MocktailDto GetMocktailById(GetbyidMocktailQuery query)
    {
        return _getByIdMocktailHandler.Handle(query);
    }
    
    public List<MocktailDto> GetAllMocktails(GetAllMocktailQuery query)
    {
        return _getAllMocktailsHandler.Handle(query);
    }
}