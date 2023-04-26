using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models
{
    public partial class ActivitiesDetail
    {
        public int IdBuilding { get; set; }
        public List<int>? Apartments { get; set; }
        public List<int>? Areas { get; set; }
        public List<int>? Activities { get; set; }
        public List<int>? Elements { get; set; }
        public List<int>? SubElements { get; set; }
        public int? StatusOption { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool? WithActivities { get; set; }
    }
}
