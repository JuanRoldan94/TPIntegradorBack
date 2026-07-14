using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TPIntegradorBack.Models
{
    public class Cliente
    {
        [Key]
        public int ClienteId { get; set; }
        [Required (ErrorMessage = "Razon Social Obligatoria")]
        public string? RazonSocial { get; set; }
        [Required (ErrorMessage = "DNI Obligatorio")]
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
