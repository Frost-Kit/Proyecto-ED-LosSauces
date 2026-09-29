using CafeteriaAromas.Data;
using CafeteriaAromas.DataStructures.Clases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaAromas.Controllers;
/// <summary>
/// Controlador responsable de gestionar la vista del Árbol General.
/// Extrae la jerarquía de categorías, productos, recetas e inventario desde la base de datos
/// y la estructura en un árbol genérico interactivo para su visualización.
/// </summary>
public class CategoryTreeController : Controller
{
    // GET
    private readonly CafeteriaDbContext _dbContext;

    public CategoryTreeController(CafeteriaDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    /// <summary>
    /// Acción principal HTTP GET que se ejecuta al ingresar a /CategoryTree.
    /// Consulta la base de datos de forma asíncrona, calcula el límite de producción 
    /// según el inventario actual y ensambla la estructura jerárquica final.
    /// </summary>
    /// <returns>Retorna la vista Index pasándole el Nodo Raíz del árbol generado.</returns>
    public async Task<IActionResult> Index()
    {
        //  Extracción de datos en cascada, para traer todos los datos requeridos de las tablas
        var categorias = await _dbContext.ProductCategories
            .Include(c => c.Products)
            .ThenInclude(p => p.Recipes)
            .ThenInclude(r => r.RecipeSupplies)
            .ThenInclude(rs => rs.Supply)
            .ToListAsync();

        var deleteProducts = await ProductsController.GetDeletedIdsAsync();
        
        ArbolGeneral<string> miArbol = new ArbolGeneral<string>("Menú de la Cafetería");
        // Comienza el llenado del árbol
        foreach (var categoria in categorias)
        {
            miArbol.AgregarNodo("Menú de la Cafetería", categoria.Name);
            foreach (var producto in categoria.Products)
            {
                if(deleteProducts.Contains(producto.Id)) continue;
                
                var receta = producto.Recipes.FirstOrDefault();
                int maxPreparables = 0;
    
                
                if (receta != null && receta.RecipeSupplies.Any())
                {
                    maxPreparables = int.MaxValue;
                    foreach (var ingrediente in receta.RecipeSupplies)
                    {
                        if (ingrediente.IngredientQuantity > 0) // Evitamos división por cero
                        {
                            // Dividimos stock entre lo requerido
                            int posibles = (int)(ingrediente.Supply.StoredQuantity / ingrediente.IngredientQuantity);
                
                            if (posibles < maxPreparables)
                            {
                                maxPreparables = posibles; // Guardamos el cuello de botella
                            }
                        }
                    }
                }

                // Creamos el nodo del producto incluyendo el cálculo
                string nombreNodoProducto = $"{producto.Name} (Máx. a preparar: {maxPreparables})";
                miArbol.AgregarNodo(categoria.Name, nombreNodoProducto);

                //  Agregamos las instrucciones y los ingredientes como hijos del producto
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