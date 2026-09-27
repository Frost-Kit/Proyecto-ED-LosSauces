using Microsoft.AspNetCore.Mvc;

namespace CafeteriaAromas.Controllers;

public class CategoryTreeController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}