namespace TPIntegradorBack.Models
{
    public class Cliente
    {
        public int ClienteID { get; set; }
        public string RazonSocial { get; set; }
        public int DNI { get; set; }
        public int Telefono { get; set; }
        public bool Activo { get; set; } = false;

        public ICollection<Direccion>? Direcciones { get; set; }
        public ICollection<Pedido>? Pedidos { get; set; }

        public Cliente() { }
    }
}
