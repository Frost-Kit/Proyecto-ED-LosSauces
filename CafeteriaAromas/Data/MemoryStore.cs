using CafeteriaAromas.Models;

namespace CafeteriaAromas.Data;


public static class MemoryStore
{
    public static bool PenditOrder = false;
    public static Queue<Sale> ActualOrders = new();
     
}