namespace Shared.Models
{
    public class ReporteActividadUsuarios
    {
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string? Usuario { get; set; }
        public List<ActividadUsuarios>? ActividadUsuario { get; set; }

    }
}
