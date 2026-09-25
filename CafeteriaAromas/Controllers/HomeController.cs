using CafeteriaAromas.Data;
using Microsoft.AspNetCore.Mvc;
using CafeteriaAromas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace CafeteriaAromas.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly CafeteriaDbContext _dbContext;
    
    public HomeController(CafeteriaDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    // GET
    public async Task<IActionResult> Index()
    {
        List<Product> actualProducts = await _dbContext.Products.ToListAsync();
        return View(actualProducts);
    }
    
    // GET
    public async Task<IActionResult> QuitSession()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login","Access");
    }
}