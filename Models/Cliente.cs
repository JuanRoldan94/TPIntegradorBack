namespace TPIntegradorBack.Models
{
    public class Cliente
    {
        public int ClienteId { get; set; }
        public string? RazonSocial { get; set; }
        public string? DNI { get; set; }
        public string? Telefono { get; set; }
        public bool Activo { get; set; } = true;
        public ICollection<Direccion>? Direcciones { get; set; }
        public ICollection<Pedido>? Pedidos { get; set; }

        //Coneccion del cliente con su cuenta
        public string? IdentityUserId { get; set; }
        public Cliente() { }
    }
}
