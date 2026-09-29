using CafeteriaAromas.Data;
using CafeteriaAromas.Models;
using CafeteriaAromas.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaAromas.Controllers;

[Authorize]
public class ProductsController : Controller
{
    private readonly CafeteriaDbContext _dbContext;
    private const string _filePath = "DeleteProducts.txt";

    public ProductsController(CafeteriaDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    /// <summary>
    /// Get de la view principal, que carga la data de la DB a una lista de ProductsViewModels,
    /// asi solo se usa lo que necesita la view,
    /// filtrando tambien los productos que esten en la "lista negra"
    /// </summary>
    /// <returns>Una view con su viewModel ya con los datos cargdos</returns>
    public async Task<IActionResult> Index()
    {
        var deletedIds = await GetDeletedIdsAsync();

        var products = await _dbContext.Products
            .Include(p => p.ProductCategory)
            .Where(p => !deletedIds.Contains(p.Id))
            .Select(p => new ProductListViewModel
            {
                Id = p.Id,
                Name = p.Name,
                SellingPrice = p.SellingPrice,
                ProductionCost = p.ProductionCost,
                CategoryName = p.ProductCategory != null ? p.ProductCategory.Name : "Sin Categoría"
            })
            .ToListAsync();

        return View(products);
    }
    
    /// <summary>
    /// GET, pal form de nuevo producto
    /// </summary>
    /// <returns>La view con el ViewModel con los items del select para la categoria</returns>
    public async Task<IActionResult> NewProduct()
    {
        var viewModel = new ProductViewModel
        {
            Categories = await GetCategorySelectListAsync()
        };

        return View(viewModel);
    }
    
    /// <summary>
    /// POST, para agregar un nuevo producto
    /// </summary>
    /// <param name="model">el viewModel con los datos ingresados en la view</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> NewProduct(ProductViewModel model)
    {
        if (ModelState.IsValid)
        {
            var product = new Product
            {
                Name = model.Name,
                SellingPrice = model.SellingPrice,
                ProductionCost = model.ProductionCost,
                ProductCategoryId = model.ProductCategoryId
            };

            _dbContext.Add(product);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        model.Categories = await GetCategorySelectListAsync();
        return View(model);
    }
     
    /// <summary>
    /// GET, pal form de actualizar/editar producto
    /// </summary>
    /// <param name="id">el id pa buscarlo en DB</param>
    /// <returns>Una view con el un objeto ProductViewModel con la data necesaria</returns>
    public async Task<IActionResult> UpdateProduct(int id)
    {
        var product = await _dbContext.Products.FindAsync(id);

        if (product == null)
            return NotFound();

        // se crea la VM pa pasarla a la view
        var viewModel = new ProductViewModel
        {
            Id = product.Id,
            Name = product.Name,
            SellingPrice = product.SellingPrice,
            ProductionCost = product.ProductionCost,
            ProductCategoryId = product.ProductCategoryId,
            Categories = await GetCategorySelectListAsync()
        };

        return View(viewModel);
    }
     
    
    /// <summary>
    /// POST, que actualiza los datos del producto elegido, con los datos que ponga en el Form
    /// Con sus validaciones, si el model no es valido Redirecciona de nuevo al form Actualizar
    /// Si no encuentra al producto redirecciona al NotFound()
    /// </summary>
    /// <param name="model">el viewModel necesario</param>
    /// <returns>Redirecciona a la lista de productos</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateProduct(ProductViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Categories = await GetCategorySelectListAsync();
            return View(model);
        }

        var product = await _dbContext.Products.FindAsync(model.Id);

        if (product == null)
            return NotFound();
        
        product.Name = model.Name;
        product.SellingPrice = model.SellingPrice;
        product.ProductionCost = model.ProductionCost;
        product.ProductCategoryId = model.ProductCategoryId;
        
        await _dbContext.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
     
    /// <summary>
    /// Metodo que "elimina" al producto elegido, solo agrega el id de producto a la "Lista Negra"
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Un redireccion a la lista</returns>
    [HttpGet]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var deletedIds = await GetDeletedIdsAsync();
        
        if (deletedIds.Add(id))
            await System.IO.File.AppendAllLinesAsync(_filePath, [ id.ToString() ]);

        return RedirectToAction(nameof(Index));
    }
    
    // metodos pa seguir el "DRY: Don't Repeat Yourself"

    /// <summary>
    /// pa que traiga las categorias al ViewModel, asi no repito codigo
    /// </summary>
    /// <returns></returns>
    private async Task<IEnumerable<SelectListItem>> GetCategorySelectListAsync()
    {
        return await _dbContext.ProductCategories
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            })
            .ToListAsync();
    }
    
    /// <summary>
    /// Para leer en un hashSet los ids de los productos eliminados, asi los podemos filtar en la LINQ del Index()
    /// igual pa no repetir codigo
    /// </summary>
    /// <returns>Un hashSet de los id de los productos</returns>
    public static async Task<HashSet<int>> GetDeletedIdsAsync()
    {
        HashSet<int> deletedIds = [];

        if (System.IO.File.Exists(_filePath))
        {
            var lines = await System.IO.File.ReadAllLinesAsync(_filePath);
            foreach (var line in lines)
            {
                if (int.TryParse(line, out int id))
                    deletedIds.Add(id);
            }
        }

        return deletedIds;
    }
}