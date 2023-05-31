using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models
{
    public class ActivityProgressByAparment
    {
        public string ApartmentNumber { get; set; }
        public string Activity_ { get; set; }
        public double ApartmentProgress { get; set; }

        public double ActivityCostTotal { get; set; }
        public double ActivitytCost { get; set; }
    }
}
