using System.Security.Cryptography.X509Certificates;

namespace TPIntegradorBack.Models
{
    public class Pedido
    {
        public int IdPedido { get; set; }
        public DateTime Fecha { get; set; }
        public decimal MontoTotal { get; set; }
        public bool Confirmado { get; set; } = false;
        public Cliente Cliente { get; set; }
        public Usuario Usuario { get; set; }

        public ICollection<DetallePedido> DetallePedido { get; set; }
        public Pedido() { }
    }
}
