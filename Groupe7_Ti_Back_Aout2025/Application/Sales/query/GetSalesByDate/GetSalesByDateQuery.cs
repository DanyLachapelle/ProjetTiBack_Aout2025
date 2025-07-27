namespace Application.Sales.query.GetSalesByDate;

public class GetSalesByDateQuery
{
    public DateTime Date { get; }
    public bool IncludeItems { get; }

    public GetSalesByDateQuery(DateTime date, bool includeItems = false)
    {
        Date = date;
        IncludeItems = includeItems;
    }
}