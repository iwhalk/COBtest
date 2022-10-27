using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class ReporteOperacionesDetalle
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
        public List<OperacionDetalle>? OperacionesDetalle { get; set; }
    }
}
