using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TPIntegradorBack.Models
{
    public class Pedido
    {
        [Key]
        public int PedidoId { get; set; }
        public DateTime Fecha { get; set; }
        public decimal MontoTotal { get; set; }
        public bool Confirmado { get; set; } = false;
        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }
        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        [ForeignKey("Direccion")]
        public int? DireccionId { get; set; }
        public Cliente Cliente { get; set; }
        public Usuario Usuario { get; set; }
        public Direccion Direccion { get; set; }

        public ICollection<DetallePedido> DetallePedido { get; set; }
        public Pedido()
        {
            DetallePedido = new List<DetallePedido>();
        }
    }
}
