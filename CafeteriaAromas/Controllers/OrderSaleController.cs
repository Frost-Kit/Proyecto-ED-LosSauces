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
    
    // GET
    public IActionResult OrdersMenu()
    {
        var orders = MemoryStore.ActualOrders.ToList();
        return View(orders);
    }

    [HttpPost]
    public IActionResult NewOrder()
    {
        // CORRECCIÓN: Creamos el tipo de objeto correcto que espera tu ColaDinamica
        var nuevaOrden = new OrderViewModel
        {
            Id = MemoryStore.ActualOrders.Size() > 0 ? 101 + MemoryStore.ActualOrders.Size() : 101,
            Cliente = "Mostrador",
            Detalle = "Nueva Orden",
            Hora = DateTime.Now.ToString("hh:mm tt"),
            Total = 0.00m,
            Estado = "En Cola"
        };

        // Insertamos la estructura compatible con tu interfaz de estructuras de datos
        MemoryStore.ActualOrders.Enqueue(nuevaOrden);
        MemoryStore.PenditOrder = true;
        
        return RedirectToAction("Index", "Menu");
    }
}
