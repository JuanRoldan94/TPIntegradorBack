namespace TPIntegradorBack.Models
{
    public class Pedido
    {
        public int PedidoId { get; set; }
        public DateTime Fecha { get; set; }
        public decimal MontoTotal { get; set; }
        public bool Confirmado { get; set; } = false;
        public int ClienteId { get; set; }
        public int UsuarioId { get; set; }
        public Cliente Cliente { get; set; }
        public Usuario Usuario { get; set; }

        public ICollection<DetallePedido> DetallePedido { get; set; }
        public Pedido()
        {
            DetallePedido = new List<DetallePedido>();
        }
    }
}
