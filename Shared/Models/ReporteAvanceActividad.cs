namespace SharedLibrary.Models
{
    public class ReporteAvanceActividad
    {
        public DateTime FechaGeneracion { get; set; }
        public List<ActivityProgress> Activities { get; set; }
    }
}
