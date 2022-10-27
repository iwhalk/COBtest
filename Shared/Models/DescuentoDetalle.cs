using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class DescuentoDetalle
    {
        public string? Tag { get; set; }
        public string? Placa { get; set; }
        public string? NoEconomico { get; set; }
        public string? Cuerpo { get; set; }
        public DateTime Fecha { get; set; }
        public string? Carril { get; set; }
        public string? Clase { get; set; }
        public string? Tarifa { get; set; }
        public string? TarifaDesc { get; set; }
        public bool Descuento { get; set; }

    }
}
