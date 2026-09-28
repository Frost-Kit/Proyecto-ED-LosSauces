using CafeteriaAromas.Data;
using CafeteriaAromas.DataStructures.Clases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaAromas.Controllers;

public class CategoryTreeController : Controller
{
    // GET
    private readonly CafeteriaDbContext _dbContext;

    public CategoryTreeController(CafeteriaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Index()
    {
        var categorias = await _dbContext.ProductCategories
            .Include(c => c.Products)
            .ThenInclude(p => p.Recipes)
            .ThenInclude(r => r.RecipeSupplies)
            .ThenInclude(rs => rs.Supply)
            .ToListAsync();

        ArbolGeneral<string> miArbol = new ArbolGeneral<string>("Menú de la Cafetería");

        foreach (var categoria in categorias)
        {
            miArbol.AgregarNodo("Menú de la Cafetería", categoria.Name);
            foreach (var producto in categoria.Products)
            {
                miArbol.AgregarNodo(categoria.Name, producto.Name);
                var receta = producto.Recipes.FirstOrDefault();
                if (receta != null)
                {
                    foreach (var ingrediente in receta.RecipeSupplies)
                    {
                        string nombreInsumo = ingrediente.Supply.Name;
                        decimal cantidadNecesaria = ingrediente.IngredientQuantity;
                        decimal cantidadDisponible = ingrediente.Supply.StoredQuantity;
                        string unidad = ingrediente.Supply.UnitOfMeasure;
                        
                        string textoNodo = $"{nombreInsumo} (Requiere: {cantidadNecesaria} {unidad} | Stock: {cantidadDisponible} {unidad})";
                        
                        miArbol.AgregarNodo(producto.Name, textoNodo);
                    }
                }
            }
        }

        return View(miArbol.Raiz);
    }
}