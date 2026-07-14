using System.ComponentModel.DataAnnotations;
namespace TPIntegradorBack.Models
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }
        [Required (ErrorMessage = "La descripción es Obligatoria")]
        public string? Descripcion { get; set; }
        [Required (ErrorMessage = "El precio es Obligatorio")]
        public decimal? PrecioUnitario { get; set; }
        [Required (ErrorMessage = "El Stock Obligatorio")]
        public int? Stock { get; set; }

        public string? Detalles { get; set; } 
        public Producto() { }

    }
}
