using Microsoft.AspNetCore.Mvc;
using CafeteriaAromas.ViewModels;
using CafeteriaAromas.Data;
using CafeteriaAromas.Models;
using Microsoft.AspNetCore.Authorization;

namespace CafeteriaAromas.Controllers;

[Authorize]
public class OrderSaleController : Controller
{
    private readonly CafeteriaDbContext _dbContext;
   
    public OrderSaleController(CafeteriaDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    // GET
    public IActionResult OrdersMenu()
    {
        var orders = MemoryStore.ActualOrders.ToList();
        return View(orders);
    }

    [HttpPost]
    public IActionResult NewOrder()
    {
        MemoryStore.ActualOrders.Enqueue(new Sale());
        MemoryStore.PenditOrder = true;
        return RedirectToAction("Index", "Menu");
    }
}