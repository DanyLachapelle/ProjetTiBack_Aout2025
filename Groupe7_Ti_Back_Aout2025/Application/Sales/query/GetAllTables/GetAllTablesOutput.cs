namespace Application.Sales.query.GetAllTables;

public class GetAllTablesOutput
{
    public List<TableDto> Tables { get; set; } = new();
}

public class TableDto
{
    public string TableNumber { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public bool IsAvailable { get; set; } = true;
}
