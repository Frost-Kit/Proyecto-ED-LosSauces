namespace CafeteriaAromas.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string Cliente { get; set; }
        public string Detalle { get; set; }
        public string Hora { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; }
    }
}
