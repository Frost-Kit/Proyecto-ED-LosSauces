using System.ComponentModel.DataAnnotations;
using CafeteriaAromas.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CafeteriaAromas.ViewModels;

public class ProductViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
    [Display(Name = "Nombre del Producto")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "El precio de venta es obligatorio.")]
    [Range(0.01, 10000.00, ErrorMessage = "Ingrese un precio válido.")]
    [Display(Name = "Precio de Venta")]
    public decimal SellingPrice { get; set; }

    [Required(ErrorMessage = "El costo de producción es obligatorio.")]
    [Range(0.00, 10000.00, ErrorMessage = "Ingrese un costo válido.")]
    [Display(Name = "Costo de Producción")]
    public decimal ProductionCost { get; set; }

    [Required(ErrorMessage = "Debe seleccionar una categoría.")]
    [Display(Name = "Categoría")]
    public int ProductCategoryId { get; set; }

    // Propiedad auxiliar para llenar el <select> en la vista
    public IEnumerable<SelectListItem>? Categories { get; set; }
}