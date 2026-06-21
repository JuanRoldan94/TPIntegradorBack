namespace TPIntegradorBack.Models
{
    public class DetallePedido
    {
        public int DetallePedidoId { get; set; }
        public int PedidoId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal CostoUnitarioHistorico { get; set; }
        public decimal Subtotal { get; set; }

        public Pedido Pedido { get; set; }
        public Producto Producto { get; set; }


        public DetallePedido() { }
    }    
}
