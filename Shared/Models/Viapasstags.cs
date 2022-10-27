using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Shared.Models
{
    public partial class Viapasstags
    {
        public Viapasstags()
        {
            TagList = new HashSet<TagList>();
        }

        public int IdViatags { get; set; }
        public string Tag { get; set; }
        public bool Active { get; set; }
        public DateTime UpdateDate { get; set; }

        public virtual ICollection<TagList> TagList { get; set; }
    }
}
