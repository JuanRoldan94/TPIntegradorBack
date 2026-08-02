using System.ComponentModel.DataAnnotations;

namespace TPIntegradorBack.ViewModels
{
    public class ClienteYDireccionViewModel
    {
        [Required(ErrorMessage = "La Razon Social es obligatoria")]
        public string? RazonSocial { get; set; }
        [Required(ErrorMessage = "El DNI es obligatorio")]
        public string? DNI { get; set; }
        public string? Telefono { get; set; }
        public bool Activo { get; set; } = true;

        public string? Calle { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "El numero no puede ser Negativo")]
        public int? Numero { get; set;  }
        public string? Localidad { get; set; }
    }
}
