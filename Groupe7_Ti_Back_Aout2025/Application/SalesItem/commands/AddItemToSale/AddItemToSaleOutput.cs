namespace Application.SalesItem.commands.AddItemToSale;

public record AddItemToSaleOutput(
    int ItemId,
    decimal NewTotalAmount
);