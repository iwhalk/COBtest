using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class RoleModules
    {
        public string RoleName { get; set; }
        public IEnumerable<int> Modules { get; set; }
    }
}
