namespace TPIntegradorBack.Models
{
    public class Direccion
    {
        public int DireccionId { get; set; }
        public string? Calle { get; set; }
        public int Numero { get; set; }
        public string? Localidad { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public Direccion() { }
    }
}
