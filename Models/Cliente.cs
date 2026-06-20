namespace TPIntegradorBack.Models
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        public string RazonSocial { get; set; }
        public int DNI { get; set; }
        public int Telefono { get; set; }
        public bool Activo { get; set; }

        public Cliente() { }
    }
}
