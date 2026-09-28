using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using CafeteriaAromas.ViewModels;

namespace CafeteriaAromas.Controllers
{
    public class OrdersController : Controller
    {
        // Cola global temporal en memoria del servidor
        private static List<OrderViewModel> ColaDeOrdenes = new List<OrderViewModel>
        {
            new OrderViewModel { Id = 101, Cliente = "Mesa 3", Detalle = "1x Cappuccino + 1x Brownie de Chocolate", Hora = "10:15 AM", Total = 42.00m, Estado = "Preparando" }
        };

        // Muestra la lista en la pantalla de Órdenes/Ventas (por hacer)
        public IActionResult Index()
        {
            return View(ColaDeOrdenes);
        }

        // Recibe los datos enviados desde el formulario de tu menú
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

            // Redirige de vuelta al menú para seguir agregando pedidos cómodamente
            return RedirectToAction("Index", "Menu");
        }

        [HttpPost]
        public IActionResult Despachar(int id)
        {
            if (ColaDeOrdenes.Count > 0)
            {
                // Obtenemos de forma estricta la primera orden de la lista (la de arriba)
                var primeraOrden = ColaDeOrdenes[0];

                // Solo si el ID enviado coincide con la primera de la lista ejecutamos la acción
                if (primeraOrden.Id == id)
                {
                    if (primeraOrden.Estado == "En Cola")
                    {
                        // Primer clic: cambia el estado
                        primeraOrden.Estado = "Preparando";
                    }
                    else if (primeraOrden.Estado == "Preparando")
                    {
                        // Segundo clic: se elimina y la de abajo sube automáticamente
                        ColaDeOrdenes.RemoveAt(0);
                    }
                }
            }

            return RedirectToAction("Index");
        }



    }
}
