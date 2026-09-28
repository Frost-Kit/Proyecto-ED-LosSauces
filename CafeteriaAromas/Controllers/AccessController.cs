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
    
    /// <summary>
    /// GET
    /// El login, si esta autenticado (que se guadaron las cookies, creo...)
    /// lo redirige al home, funciona...
    /// </summary>
    /// <returns>La view con el login pa que ingrese</returns>
    public IActionResult Login()
    {
        if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
        
        return View();
    }
    
    /// <summary>
    /// POST
    /// Se encarga de procesar el inicio de sesion, comprueba, si existe un usuario/empleado
    /// con los datos ingresados, si hya lo deja pasar e "inicia sesion" mediante cookies,
    /// (magia negra)
    /// </summary>
    /// <param name="model"> recibe un objeto viewModel correspondiente con los datos de la view</param>
    /// <returns>Redirige al home</returns>
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

    /// <summary>
    /// Un metodo pal nombre completo del esclavo/empleado
    /// </summary>
    /// <param name="employee"></param>
    /// <returns>una cadena con su nombre completo</returns>
    private string FullName(Employee employee) => $"{employee.FirstName} {employee.LastName}";
}