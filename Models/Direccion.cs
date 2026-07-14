using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TPIntegradorBack.Models
{
    public class Direccion
    {
        [Key]
        public int DireccionId { get; set; }
        [Required(ErrorMessage = "Calle Obligatoria")]
        public string? Calle { get; set; }
        [Required(ErrorMessage = "Número Obligatorio")]
        public int? Numero { get; set; }
        [Required (ErrorMessage = "Localidad Obligatoria")]
        public string? Localidad { get; set; }
        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public Direccion() { }
    }
}
