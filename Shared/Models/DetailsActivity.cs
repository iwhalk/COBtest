using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models
{
    public partial class DetailsActivity
    {
        public int idBuilding { get; set; }
        public List<int>? idActivities { get; set; }        
        public List<int>? idElements { get; set; }
        public List<int> idApartments { get; set; }
    }
}
