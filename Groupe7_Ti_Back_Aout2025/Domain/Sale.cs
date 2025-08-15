using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public class Sale
{
    [Column ("id")]
    public int Id { get; set; }
    
    [Column ("total_amount")]
    public decimal TotalAmount { get; set; }
    
    [Column ("sale_date")]
    public DateTime SaleDate { get; set; } = DateTime.Now;
    
    [Column ("table_number")]
    public string TableNumber { get; set; }
    
    [Column ("status")]
    public string Status { get; set; } = "Pending";
    
    [Column ("order_timer")]
    public int OrderTimer { get; set; } = 15; 
    // Navigation property
    public virtual ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
    
    
}