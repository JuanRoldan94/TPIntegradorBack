namespace TPIntegradorBack.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string? Descripcion { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Stock { get; set; }

        public string? Detalles { get; set; } 
        public Producto() { }

    }
}
