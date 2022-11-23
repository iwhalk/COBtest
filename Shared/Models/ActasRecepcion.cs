using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models
{
    public class ActasRecepcion
    {
        public int IdReceptionCertificate { get; set; }
        public DateTime Fecha { get; set; }
        public string Acta { get; set; }
        public string Inmueble { get; set; }
        public int Habitaciones { get; set; }
        public string Arrendador { get; set; }
        public string Arrendatario { get; set; }
        public string Delegacion { get; set; }
        public string? Agente { get; set; }
        public int IdProperty { get; set; }

    }
}
