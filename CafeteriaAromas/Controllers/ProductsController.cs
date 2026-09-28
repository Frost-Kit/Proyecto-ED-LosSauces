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
   
    public ProductsController(CafeteriaDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    // GET: Products
    public async Task<IActionResult> Index()
    {
        var products = await _dbContext.Products
            .Include(p => p.ProductCategory)
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
     
    [HttpGet]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _dbContext.Products.FirstAsync(e => e.Id == id);
        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    
    /// <summary>
    /// pa que traiga las categorias al ViewModel, asi no repito codigo
    /// </summary>
    /// <returns></returns>
    private async Task<IEnumerable<SelectListItem>> GetCategorySelectListAsync() 
        => await _dbContext.ProductCategories
                 .Select(c => new SelectListItem
                 {
                    Value = c.Id.ToString(),
                    Text = c.Name
                 })
                 .ToListAsync();
}