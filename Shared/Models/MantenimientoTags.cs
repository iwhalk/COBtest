using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class MantenimientoTags
    {
        [JsonPropertyName("paginas_totales")]
        public int? PaginasTotales { get; set; }

        [JsonPropertyName("pagina_actual")]
        public int? PaginaActual { get; set; }
        public List<TagList>? Tags { get; set; }
    }
}
