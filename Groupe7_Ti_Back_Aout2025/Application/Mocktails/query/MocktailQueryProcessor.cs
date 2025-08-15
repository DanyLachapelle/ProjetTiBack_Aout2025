using System.Collections.Generic;
using Application.DTOs;
using Application.Mocktails.query.getAllMocktail;
using Application.Mocktails.query.getbyidMocktail;
using Application.Utils;

namespace Application.Mocktails.query;

// Central query processing facade for mocktail-related queries
public class MocktailQueryProcessor
{
    // Handlers for specific query types
    private readonly IQueryHandler<GetbyidMocktailQuery, MocktailDto> _getByIdMocktailHandler;
    private readonly IQueryHandler<GetAllMocktailQuery, List<MocktailDto>> _getAllMocktailsHandler;
    
    // Constructor with dependency injection
    public MocktailQueryProcessor(
        IQueryHandler<GetbyidMocktailQuery, MocktailDto> getByIdMocktailHandler,
        IQueryHandler<GetAllMocktailQuery, List<MocktailDto>> getAllMocktailsHandler)
    {
        _getByIdMocktailHandler = getByIdMocktailHandler;
        _getAllMocktailsHandler = getAllMocktailsHandler;
    }
    
    // Retrieves a single mocktail by ID
    public MocktailDto GetMocktailById(GetbyidMocktailQuery query)
    {
        // Note: Consider adding null/validation checks here
        return _getByIdMocktailHandler.Handle(query);
    }
    
    // Retrieves all mocktails
    public List<MocktailDto> GetAllMocktails(GetAllMocktailQuery query)
    {
        // Note: Consider adding pagination/filtering support
        return _getAllMocktailsHandler.Handle(query);
    }
}