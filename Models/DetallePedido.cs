using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TPIntegradorBack.Models
{
    public class DetallePedido
    {
        [Key]
        public int DetallePedidoId { get; set; }
        [ForeignKey("Pedido")]
        public int PedidoId { get; set; }
        [ForeignKey("Producto")]
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal CostoUnitarioHistorico { get; set; }
        public decimal Subtotal { get; set; }

        public Pedido Pedido { get; set; }
        public Producto Producto { get; set; }


        public DetallePedido() { }
    }    
}
