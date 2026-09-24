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
    
    // GET
    public async Task<IActionResult> Index()
    {
        List<Product> actualProducts = await _dbContext.Products.ToListAsync();
        return View(actualProducts);
    }
    
}