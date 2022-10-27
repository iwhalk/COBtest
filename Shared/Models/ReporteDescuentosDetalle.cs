namespace Shared.Models
{
    public class ReporteDescuentosDetalle
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public List<DescuentoDetalle>? DescuentosDetalle { get; set; }
    }
}
