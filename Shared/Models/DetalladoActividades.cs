using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models
{
    public class DetalladoActividades
    {
        public string actividad { get; set; }
        public string elemento { get; set; }
        public string subElemento { get; set; }
        public string estatus { get; set; }
        public string total { get; set; }
        public int avance { get; set; }
    }
}
