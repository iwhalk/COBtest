namespace Shared.Models
{
    public class ReporteArrendadores
    {
        public string? NumeroArrendadores { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public List<ArrendadoresDetalle?> Arrendadores { get; set; }
    }
}
