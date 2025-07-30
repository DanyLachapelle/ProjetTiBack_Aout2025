namespace Domain;

public class Sale
{
    public int Id { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime SaleDate { get; set; } = DateTime.Now;
    public string TableNumber { get; set; }

    // Navigation property
    public virtual ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
}