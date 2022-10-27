namespace Shared.Models
{
    public class ReporteDescuentosResumen
    {
        public string? DireccionEntrada { get; set; }
        public string? DireccionSalida { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public List<DescuentoResumen>? DescuentosResumen { get; set; }
    }
}
