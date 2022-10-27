namespace Shared.Models
{
    public class ReporteTransaccionesOperativo
    {
        public string? Plaza { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public List<TransaccionOperativoDetalle>? TransaccionesOperativoDetalle { get; set; }

    }
}