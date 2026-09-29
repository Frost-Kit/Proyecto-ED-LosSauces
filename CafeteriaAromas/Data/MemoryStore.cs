using CafeteriaAromas.ViewModels;
using CafeteriaAromas.DataStructures.Clases;

namespace CafeteriaAromas.Data
{
    public static class MemoryStore
    {
        public static bool PenditOrder = false;
        public static ColaDinamica<OrderViewModel> ActualOrders = new ColaDinamica<OrderViewModel>();
    }
}
