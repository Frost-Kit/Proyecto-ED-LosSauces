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
                var receta = producto.Recipes.FirstOrDefault();
                int maxPreparables = 0;
    
                // 1. Calcular cuántos productos se pueden hacer según el inventario
                if (receta != null && receta.RecipeSupplies.Any())
                {
                    maxPreparables = int.MaxValue;
                    foreach (var ingrediente in receta.RecipeSupplies)
                    {
                        if (ingrediente.IngredientQuantity > 0) // Evitamos división por cero
                        {
                            // Dividimos stock entre lo requerido y redondeamos hacia abajo
                            int posibles = (int)Math.Floor(ingrediente.Supply.StoredQuantity / ingrediente.IngredientQuantity);
                
                            if (posibles < maxPreparables)
                            {
                                maxPreparables = posibles; // Guardamos el cuello de botella
                            }
                        }
                    }
                }

                // 2. Creamos el nodo del producto incluyendo el cálculo
                string nombreNodoProducto = $"{producto.Name} (Máx. a preparar: {maxPreparables})";
                miArbol.AgregarNodo(categoria.Name, nombreNodoProducto);

                // 3. Agregamos las instrucciones y los ingredientes como hijos del producto
                if (receta != null)
                {
                    // Nodo de la receta completa
                    miArbol.AgregarNodo(nombreNodoProducto, $"Instrucciones: {receta.Instructions}");

                    // Nodos de los ingredientes
                    foreach (var ingrediente in receta.RecipeSupplies)
                    {
                        string nombreInsumo = ingrediente.Supply.Name;
                        decimal cantNec = ingrediente.IngredientQuantity;
                        decimal cantDisp = ingrediente.Supply.StoredQuantity;
                        string unidad = ingrediente.Supply.UnitOfMeasure;

                        string textoIngrediente = $"Ingrediente: {nombreInsumo} (Requiere: {cantNec} {unidad} | Stock: {cantDisp} {unidad})";
                        miArbol.AgregarNodo(nombreNodoProducto, textoIngrediente);
                    }
                }
            }
        }

        return View(miArbol.Raiz);
    }
}