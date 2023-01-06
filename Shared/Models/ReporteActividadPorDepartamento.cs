using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models
{
    public class ReporteActividadPorDepartamento
    {
        public string Actividad { get; set; }
        public List<AparmentProgress> Apartments { get; set; }
    }
}
