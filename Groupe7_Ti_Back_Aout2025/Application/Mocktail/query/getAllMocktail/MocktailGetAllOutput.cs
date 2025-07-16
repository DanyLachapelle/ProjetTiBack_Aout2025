using Application.DTOs;

namespace Application.Mocktail.query.getAllMocktail;

public class MocktailGetAllOutput
{
    public List<MocktailDto> Mocktails { get; set; } = new List<MocktailDto>();
}