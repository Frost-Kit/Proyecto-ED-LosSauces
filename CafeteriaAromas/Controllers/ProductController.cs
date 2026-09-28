using CafeteriaAromas.Data;
using CafeteriaAromas.Models;
using CafeteriaAromas.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaAromas.Controllers;

[Authorize]
public class ProductController : Controller
{
    private readonly CafeteriaDbContext _dbContext;
   
    public ProductController(CafeteriaDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    [HttpGet]
    public async Task<IActionResult> ListProducts()
    {
        var products = await _dbContext.Products
            .OrderBy(p => p.Id)
            .Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                SellingPrice = p.SellingPrice,
                ProductionCost = p.ProductionCost,
                CategoryName = p.ProductCategory.Name
            })
            .ToListAsync();
        
        return View(products);
    }
    
    [HttpGet]
    public async Task<IActionResult> NewProduct()
    {
        var categoriesDict = await _dbContext.ProductCategories
            .ToDictionaryAsync(c => c.Id, c => c.Name);
        
        ViewBag.Categories = new SelectList(categoriesDict, "Key", "Value");

        return View();
    }
    
         
    [HttpPost]
    public async Task<IActionResult> NewProduct(Product product)
    {
        await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(ListProducts));
    }
     
    [HttpGet]
    public async Task<IActionResult> UpdateProduct(int id)
    {
        var product = await _dbContext.Products.FirstAsync(e => e.Id == id);
        return View(product);
    }
     
    [HttpPost]
    public async Task<IActionResult> UpdateProduct(Product product)
    {
        _dbContext.Products.Update(product);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(ListProducts));
    }
     
    [HttpGet]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _dbContext.Products.FirstAsync(e => e.Id == id);
        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(ListProducts));
    }
}