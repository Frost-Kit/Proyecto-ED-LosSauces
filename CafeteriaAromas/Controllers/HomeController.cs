using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CafeteriaAromas.Models;

namespace CafeteriaAromas.Controllers;

public class HomeController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }

  
}