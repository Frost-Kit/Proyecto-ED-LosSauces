using CafeteriaAromas.ViewModels;
using CafeteriaAromas.DataStructures.Clases;

namespace CafeteriaAromas.Data
{
    public static class MemoryStore
    {
        public static bool PenditOrder = false;
        
        // Instanciamos tu estructura personalizada utilizando tu interfaz
        public static ColaDinamica<OrderViewModel> ActualOrders = new ColaDinamica<OrderViewModel>();
    }
}
