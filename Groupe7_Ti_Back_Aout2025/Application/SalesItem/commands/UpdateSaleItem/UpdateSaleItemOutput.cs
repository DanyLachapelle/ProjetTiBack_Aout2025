namespace Application.SalesItem.commands.UpdateSaleItem;

public record UpdateSaleItemOutput(
    int UpdatedItemId,
    int NewQuantity,
    decimal NewItemTotal,
    decimal NewSaleTotal
);