using CafeteriaAromas.Data;
using Microsoft.AspNetCore.Mvc;
using CafeteriaAromas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CafeteriaAromas.Controllers;

[Authorize]
public class MenuController : Controller
{
    private readonly CafeteriaDbContext _dbContext;
    
    public MenuController(CafeteriaDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    /// <summary>
    /// GET, para la view del menu
    /// </summary>
    /// <returns>Una view con su viewModel con los datos cargados</returns>
    public async Task<IActionResult> Index()
    {
        var deletedProducts = await ProductsController.GetDeletedIdsAsync();
        
        var actualProducts = await _dbContext.Products
            .Where(p => !deletedProducts.Contains(p.Id))
            .ToListAsync();
        
        return View(actualProducts);
    }
    
    
}