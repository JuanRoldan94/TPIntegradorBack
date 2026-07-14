using System.ComponentModel.DataAnnotations;
namespace TPIntegradorBack.Models
{
    public class Usuario
    {
        [Key]
        public int UsuarioId { get; set; }
        [Required (ErrorMessage = "El nombre de usuario es obligatorio")]
        public string? NombreUsuario { get; set; }
        [Required (ErrorMessage = "La contraseña es obligatoria")]
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
