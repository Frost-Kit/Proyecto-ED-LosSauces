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

        private static List<OrderViewModel> ColaDeOrdenes = new List<OrderViewModel>
        {
            new OrderViewModel { Id = 101, Cliente = "Mostrador", Detalle = "1x Cappuccino", Hora = "10:15 AM", Total = 22.00m, Estado = "Preparando" }
        };

        public IActionResult Index()
        {
            return View(ColaDeOrdenes);
        }

        [HttpPost]
        public IActionResult AgregarDesdeMenu(int id, string nombre, decimal precio)
        {
            var nuevaOrden = new OrderViewModel
            {
                Id = ColaDeOrdenes.Count > 0 ? ColaDeOrdenes[ColaDeOrdenes.Count - 1].Id + 1 : 101,
                Cliente = "Mostrador",
                Detalle = $"1x {nombre}",
                Hora = DateTime.Now.ToString("hh:mm tt"),
                Total = precio,
                Estado = "En Cola"
            };

            ColaDeOrdenes.Add(nuevaOrden);

            return RedirectToAction("Index", "Menu");
        }

        [HttpPost]
        public IActionResult Despachar(int id)
        {
            if (ColaDeOrdenes.Count > 0)
            {
                var primeraOrden = ColaDeOrdenes[0];

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
                            System.Diagnostics.Debug.WriteLine("Error al descontar stock: " + ex.Message);
                        }

                        primeraOrden.Estado = "Preparando";
                    }
                    else if (primeraOrden.Estado == "Preparando")
                    {
                        ColaDeOrdenes.RemoveAt(0);
                    }
                }
            }

            return RedirectToAction("Index");
        }
    }
}
