namespace Shared.Models
{
    public class ReporteTransaccionesDetalle
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public List<TransaccionDetalle> TransaccionesDetalle { get; set; }
    }
}
