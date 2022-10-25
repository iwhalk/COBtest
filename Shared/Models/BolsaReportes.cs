using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class BolsaReportes
    {
        public int? IdBolsa { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string? CarrilBolsa { get; set; }
        public int? Bolsa { get; set; }
    }
}
