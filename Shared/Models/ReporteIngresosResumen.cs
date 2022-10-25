namespace Shared.Models
{
    public class ReporteIngresosResumen
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public List<IngresoResumen> IngresosResumen { get; set; }
    }
}
