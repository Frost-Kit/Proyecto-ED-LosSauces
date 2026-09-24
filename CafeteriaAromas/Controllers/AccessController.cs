using CafeteriaAromas.Data;
using CafeteriaAromas.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Security.Claims;
using CafeteriaAromas.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace CafeteriaAromas.Controllers;

public class AccessController : Controller
{
    private readonly CafeteriaDbContext  _dbContext;

    public AccessController(CafeteriaDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    // GET
    public IActionResult Login()
    {
        if (User.Identity!.IsAuthenticated) return RedirectToAction("Index", "Home");
        
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var userFound = await _dbContext.Employees
            .Where(employee => model.Email == employee.Email && model.Password == employee.DocumentId)
            .FirstOrDefaultAsync();

        if (userFound is null)
        {
            ViewData["Message"] = "No se encontraron coincidencias, Ni modo ";
            return View();
        }

        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Name, FullName(userFound)),
        };
        
        ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        AuthenticationProperties properties = new()
        {
            AllowRefresh = true,
        };
        
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), properties);
        
        return RedirectToAction("Index", "Home");
    }

    private string FullName(Employee employee) => $"{employee.FirstName} {employee.LastName}";
}