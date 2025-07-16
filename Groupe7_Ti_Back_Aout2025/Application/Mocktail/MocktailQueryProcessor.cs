using Application.Mocktail.query.getAllMocktail;
using Application.Utils;

namespace Application.Mocktail.query;

public class MocktailQueryProcessor
{
    private  readonly IQueryHandler<MocktailGetAllQuery, MocktailGetAllOutput> _mocktailGetAllHandler;
   
    public MocktailQueryProcessor(IQueryHandler<MocktailGetAllQuery, MocktailGetAllOutput> mocktailGetAllHandler)
    {
        _mocktailGetAllHandler = mocktailGetAllHandler;
    }
   
    public MocktailGetAllOutput Mocktails(MocktailGetAllQuery query)
    {
        return _mocktailGetAllHandler.Handle(query);
    }

}