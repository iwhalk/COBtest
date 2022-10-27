namespace Shared.Models
{
    public class ReporteTransacciones
    {
        public string? Plaza { get; set; }
        public string? Via { get; set; }
        public string? Bag { get; set; }
        public string? Corte { get; set; }
        public string? NoCajero { get; set; }
        public string? Nombre { get; set; }
        public string? Turno { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public List<TransaccionDetalle>? TransaccionesDetalle { get; set; }

    }
}

