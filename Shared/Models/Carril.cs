using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class Carril
    {
        public string Id { get; set; }

        [JsonPropertyName("Carril")]
        public string Nombre { get; set; }
    }
}
