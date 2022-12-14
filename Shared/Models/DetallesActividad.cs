using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models
{
    public partial class DetallesActividad
    {
        public int idBuilding { get; set; }
        public List<int> idApartments { get; set; }
        public List<int> idActivy { get; set; }
        public List<int> idElement { get; set; }
        public List<int>? idSubElements { get; set; }

    }
}
