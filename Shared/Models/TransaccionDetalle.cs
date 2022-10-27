using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class TransaccionDetalle
    {
        public string? Tag { get; set; }
        public string? Placa { get; set; }
        public string? NoEconomico { get; set; }
        public string? Cuerpo { get; set; }
        public DateTime Fecha { get; set; }
        public string? Carril { get; set; }
        public string? ClasePre { get; set; }
        public string? ClaseCajero { get; set; }
        public string? ClasePost { get; set; }
        public string? MedioPago { get; set; }
        public string? Tarifa { get; set; }
        public string? TarifaDescuento { get; set; }

    }
}
