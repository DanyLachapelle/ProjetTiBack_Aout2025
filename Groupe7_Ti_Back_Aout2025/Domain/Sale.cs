namespace Domain;

public class Sale
{
    public int Id { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime SaleDate { get; set; } = DateTime.Now;
    public string? TableNumber { get; set; }
    public string status { get; set; } = "PENDING"; 
    public int? order_timer { get; set; } = 15; 
    // Navigation property
    public virtual ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
}