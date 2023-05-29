using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models
{
    public class AparmentProgress
    {
        public string? Activity_ { get; set; }
        public string ApartmentNumber { get; set; }
        public double ApartmentProgress { get; set; }

        public double ApartmentCostTotal { get; set; }
        public double ApartmentCost { get; set; }
    }
}
