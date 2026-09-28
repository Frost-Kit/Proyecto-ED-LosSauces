using CafeteriaAromas.Data;
using Microsoft.AspNetCore.Mvc;
using CafeteriaAromas.Models;
using CafeteriaAromas.ViewModels;
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
        var viewModel = new HomeViewModel
        {
            TotalProducts = await _dbContext.Products.CountAsync(),
            TotalSuppliers = await _dbContext.Suppliers.CountAsync(),
            TotalEmployees = await _dbContext.Employees.CountAsync()
        };

        return View(viewModel);
    }
    
    /// <summary>
    /// Esto es para probar/ver los cambios en el site.css,
    /// pueden cambiar la vista (.cshtml) si queren.
    /// En el link del navegador pongan /Home/TestDesign
    /// </summary>
    /// <returns>una vista Razor?</returns>
    public IActionResult TestDesign() => View();
    
    // GET
    public async Task<IActionResult> QuitSession()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login","Access");
    }
}