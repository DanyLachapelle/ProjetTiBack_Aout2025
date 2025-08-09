using System;
using System.Collections.Generic;

namespace Application.DTOs;

public class SaleDto
{
    public int Id { get; set; }
    public string TableNumber { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime SaleDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public int order_timer { get; set; } = 15;
    public List<SaleItemDto>? Items { get; set; }
}
