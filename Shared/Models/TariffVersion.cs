using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Shared.Models
{

    [Table("TariffVersion")]
    public partial class TariffVersion
    {
        public TariffVersion()
        {
            Tariffs = new HashSet<Tariff>();
        }

        [Key]
        [Column("ID_TariffVersion")]
        public int IdTariffVersion { get; set; }
        public int Version { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreationDate { get; set; }
        public bool Active { get; set; }
        [Column("ID_Catalog")]
        public int? IdCatalog { get; set; }

        [InverseProperty("IdTariffVersionNavigation")]
        public virtual ICollection<Tariff> Tariffs { get; set; }
    }
}
