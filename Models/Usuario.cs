namespace TPIntegradorBack.Models
{
    public class Usuario
    {
        public int UsuarioId { get; set; }
        public string? NombreUsuario { get; set; }
        public string? Contrasenia { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public bool Role { get; set; }
        public bool Activo { get; set; } = true;

        //Si el Role es true. El usuario es administrador. De lo contrario, es un usuario general

        public ICollection<Pedido>? Pedidos { get; set; }
        public Usuario() { }
    }
}
