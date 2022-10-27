using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class DescuentoResumen
    {
        public string? Tag { get; set; }
        public string? Placa { get; set; }
        public string? NoEconomico { get; set; }
        public DateTime FechaEntrada { get; set; }
        public string? CarrilEntrada { get; set; }
        public string? ClaseEntrada { get; set; }
        public string? TarifaEntrada { get; set; }
        public DateTime? FechaSalida { get; set; }
        public string? CarrilSalida { get; set; }
        public string? ClaseSalida { get; set; }
        public string? TarifaSalida { get; set; }
        public string? TarifaDescuento { get; set; }
        public bool Descuento { get; set; }
    }
}
