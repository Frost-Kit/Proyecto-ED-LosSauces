namespace CafeteriaAromas.ViewModels;

public class ProductListViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal SellingPrice { get; set; }
    public decimal ProductionCost { get; set; }
        
    // en lugar de traer solo la ID, trae/guardo el Nombre de la categoría
    public string CategoryName { get; set; } = string.Empty;
}