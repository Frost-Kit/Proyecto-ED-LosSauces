using Microsoft.AspNetCore.Mvc;
using CafeteriaAromas.ViewModels;
using CafeteriaAromas.Data;
using CafeteriaAromas.Models;
using Microsoft.AspNetCore.Authorization;
using System;

namespace CafeteriaAromas.Controllers;

[Authorize]
public class OrderSaleController : Controller
{
    private readonly CafeteriaDbContext _dbContext;
   
    public OrderSaleController(CafeteriaDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public IActionResult OrdersMenu()
    {
        var orders = MemoryStore.ActualOrders.ToList();
        return View(orders);
    }

    [HttpPost]
    public IActionResult NewOrder()
    {
        var nuevaOrden = new OrderViewModel
        {
            Id = MemoryStore.ActualOrders.Size() > 0 ? 101 + MemoryStore.ActualOrders.Size() : 101,
            Cliente = "Mostrador",
            Detalle = "Nueva Orden",
            Hora = DateTime.Now.ToString("hh:mm tt"),
            Total = 0.00m,
            Estado = "En Cola"
        };

        MemoryStore.ActualOrders.Enqueue(nuevaOrden);
        MemoryStore.PenditOrder = true;
        
        return RedirectToAction("Index", "Menu");
    }
}
