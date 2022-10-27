namespace Shared.Models
{
    public class TransaccionOperativoDetalle
    {
        public string? Tag { get; set; }
        public string? Cuerpo { get; set; }
        public DateTime Fecha { get; set; }
        public string? Carril { get; set; }
        public string? ClasePre { get; set; }
        public string? ClaseCajero { get; set; }
        public string? ClasePost { get; set; }
        public string? MedioPago { get; set; }
        public string? Tarifa { get; set; }
    }
}