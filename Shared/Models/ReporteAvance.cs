using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models
{
    public class ReporteAvance
    {
        public DateTime FechaGeneracion { get; set; }
        public List<AparmentProgress> Apartments { get; set; }
    }
}
