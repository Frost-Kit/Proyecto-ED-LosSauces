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
    /// Get de la view principal, que pide el hast
    /// </summary>
    /// <returns></returns>
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
    
    // GET: para nuevos Productos
    public async Task<IActionResult> NewProduct()
    {
        var viewModel = new ProductViewModel
        {
            Categories = await GetCategorySelectListAsync()
        };

        return View(viewModel);
    }
    
         
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
     
    // GET: Products/UpdateProduct/5
    public async Task<IActionResult> UpdateProduct(int id)
    {
        var product = await _dbContext.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }

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
        {
            return NotFound();
        }
        
        product.Name = model.Name;
        product.SellingPrice = model.SellingPrice;
        product.ProductionCost = model.ProductionCost;
        product.ProductCategoryId = model.ProductCategoryId;
        
        await _dbContext.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
     
    /// <summary>
    /// Metodo que "elimina" al producto elegido
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var deletedIds = await GetDeletedIdsAsync();
        
        if (deletedIds.Add(id))
            await System.IO.File.AppendAllLinesAsync(_filePath, new[] { id.ToString() });

        return RedirectToAction(nameof(Index));
    }
    
    // metodos pa seguire el "DRY: Don't Repeat Yourself"

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
                {
                    deletedIds.Add(id);
                }
            }
        }

        return deletedIds;
    }
}