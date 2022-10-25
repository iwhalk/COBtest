using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class Bolsa
    {
        public int? Id { get; set; }
        public string Inicio { get; set; }
        public string Fin { get; set; }
        public string Carril { get; set; }
        public string Nombre { get; set; }
    }
}
