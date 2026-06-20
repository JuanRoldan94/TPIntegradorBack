namespace TPIntegradorBack.Models
{
    public class Usuario
    {
        public int UsuarioID { get; set; }
        public string NombreUsuario { get; set; }
        public string Contrasenia { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Role { get; set; }

        public ICollection<Pedido>? Pedidos { get; set; }
        public Usuario() { }
    }
}
