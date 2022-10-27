using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class ReporteCajeroEncabezado
    {
        public int IdBolsa { get; set; }
        public int NoCajero { get; set; }
        public int Turno { get; set; }
        public string Fecha { get; set; }
    }
}
