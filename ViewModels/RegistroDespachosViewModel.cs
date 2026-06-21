using System.Collections.Generic;

namespace GestorDespacho.ViewModels
{
    public class ConfirmarPedido
    {
        public int ClienteId { get; set; }
        public decimal MontoTotal { get; set; }
        public List<DetallePedido> Detalles { get; set; } = new List<DetallePedido>();
    }

    public class DetallePedido
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal CostoUnitario { get; set; }
    }
}