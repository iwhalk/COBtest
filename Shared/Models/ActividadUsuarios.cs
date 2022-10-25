namespace Shared.Models
{
    public class ActividadUsuarios
    {
        public string? Nombre { get; set; }
        public string? Rol{ get; set; }
        public DateTime? FechaMovimiento { get; set; }
        public string? Modulo  { get; set; }
        public string? Accion { get; set; }
        public string? RegistroOriginal { get; set; }
        public string? RegistroEditado { get; set; }
    }
}
