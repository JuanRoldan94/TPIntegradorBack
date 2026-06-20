namespace TPIntegradorBack.Models
{
    public class DetallePedido
    {
        public int Id { get; set; }
        public int IdPedido { get; set; }
        public int IdProducto { get; set; }
        public int cantidad { get; set; }
        public decimal CostoUnitarioHistorico { get; set; }
        public decimal Subtotal { get; set; }

        public Pedido Pedido { get; set; }
        public Producto Producto { get; set; }


        public DetallePedido() { }
    }    
}
