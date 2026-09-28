using CafeteriaAromas.Models;

namespace CafeteriaAromas.ViewModels;

public class ProductViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal ProductionCost { get; set; }
    public decimal SellingPrice { get; set; }
    public string CategoryName { get; set; }
    public int CategoryId { get; set; }
}