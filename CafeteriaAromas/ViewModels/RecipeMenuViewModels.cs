using Microsoft.AspNetCore.Mvc.Rendering;

namespace CafeteriaAromas.ViewModels;

public class RecipeManagementViewModel
{
    public int? SelectedProductId { get; set; }
    public int RecipeId { get; set; }
    public string Instructions { get; set; } = string.Empty;
    
    public List<SelectListItem> ProductList { get; set; } = [];
    public List<SelectListItem> AvailableSupplies { get; set; } = [];
    public List<RecipeItemViewModel> Ingredients { get; set; } = [];
}


public class RecipeItemViewModel
{
    public int RecipeSupplyId { get; set; }
    public int SupplyId { get; set; }
    public string SupplyName { get; set; } = string.Empty;
    public decimal IngredientQuantity { get; set; }
    public string UnitOfMeasure { get; set; } = string.Empty;
}