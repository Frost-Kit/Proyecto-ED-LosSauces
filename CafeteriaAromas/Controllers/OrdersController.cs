using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using CafeteriaAromas.ViewModels;
using CafeteriaAromas.Data;
using System.Linq;

namespace CafeteriaAromas.Controllers
{
    public class OrdersController : Controller
    {
        private readonly CafeteriaDbContext _context;

        public OrdersController(CafeteriaDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(MemoryStore.ActualOrders.ToList());
        }

        [HttpPost]
        public IActionResult AgregarDesdeMenu(int id, string nombre, decimal precio)
        {
            var nuevaOrden = new OrderViewModel
            {
                Id = MemoryStore.ActualOrders.Size() > 0 ? 101 + MemoryStore.ActualOrders.Size() : 101,
                Cliente = "Mostrador",
                Detalle = $"1x {nombre}",
                Hora = DateTime.Now.ToString("hh:mm tt"),
                Total = precio,
                Estado = "En Cola"
            };

            MemoryStore.ActualOrders.Enqueue(nuevaOrden);

            return RedirectToAction("Index", "Menu");
        }

        [HttpPost]
        public IActionResult Despachar(int id)
        {
            if (!MemoryStore.ActualOrders.IsEmpty())
            {
                var primeraOrden = MemoryStore.ActualOrders.Peek();

                if (primeraOrden.Id == id)
                {
                    if (primeraOrden.Estado == "En Cola")
                    {
                        try
                        {
                            string nombreProducto = primeraOrden.Detalle.Replace("1x ", "").Trim();
                            var producto = _context.Products.FirstOrDefault(p => p.Name == nombreProducto);

                            if (producto != null)
                            {
                                var receta = _context.Recipes.FirstOrDefault(r => r.ProductId == producto.Id);
                                if (receta != null)
                                {
                                    var insumosReceta = _context.RecipeSupplies.Where(rs => rs.RecipeId == receta.Id).ToList();

                                    foreach (var insumoReceta in insumosReceta)
                                    {
                                        var insumoStock = _context.Supplies.FirstOrDefault(s => s.Id == insumoReceta.SupplyId);
                                        if (insumoStock != null)
                                        {
                                            insumoStock.StoredQuantity -= insumoReceta.IngredientQuantity;
                                        }
                                    }
                                    _context.SaveChanges();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("Error al restar stock: " + ex.Message);
                        }

                        primeraOrden.Estado = "Preparando";
                    }
                    else if (primeraOrden.Estado == "Preparando")
                    {
                        MemoryStore.ActualOrders.Dequeue();
                    }
                }
            }

            return RedirectToAction("Index");
        }
    }
}
