namespace TPIntegradorBack.Models
{
    public class Usuario
    {
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
        public string Contrasenia { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public bool Role { get; set; }

        //Si el Role es 1. El usuario es administrador. De lo contrario, es un usuario general

        public ICollection<Pedido>? Pedidos { get; set; }
        public Usuario() { }
    }
}
