﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SharedLibrary.Models
{
    public partial class Inventory
    {
        public Inventory()
        {
            BlobsInventories = new HashSet<BlobsInventory>();
        }

        [Key]
        [Column("ID_Inventory")]
        public int IdInventory { get; set; }
        [Column("ID_Property")]
        public int IdProperty { get; set; }
        [Column("ID_Area")]
        public int IdArea { get; set; }
        [Column("ID_Description")]
        public int IdDescription { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Note { get; set; }
        [StringLength(150)]
        [Unicode(false)]
        public string Observation { get; set; }
        [Column("ID_ReceptionCertificate")]
        public int? IdReceptionCertificate { get; set; }
        [Column("ID_Service")]
        public int? IdService { get; set; }
        [Column("ID_Feature")]
        public int? IdFeature { get; set; }

        [ForeignKey("IdArea")]
        [InverseProperty("Inventories")]
        public virtual Area IdAreaNavigation { get; set; }
        [ForeignKey("IdDescription")]
        [InverseProperty("Inventories")]
        public virtual Description IdDescriptionNavigation { get; set; }
        [ForeignKey("IdFeature")]
        [InverseProperty("Inventories")]
        public virtual Feature IdFeatureNavigation { get; set; }
        [ForeignKey("IdProperty")]
        [InverseProperty("Inventories")]
        public virtual Property IdPropertyNavigation { get; set; }
        [ForeignKey("IdReceptionCertificate")]
        [InverseProperty("Inventories")]
        public virtual ReceptionCertificate IdReceptionCertificateNavigation { get; set; }
        [ForeignKey("IdService")]
        [InverseProperty("Inventories")]
        public virtual Service IdServiceNavigation { get; set; }
        [InverseProperty("IdInventoryNavigation")]
        public virtual ICollection<BlobsInventory> BlobsInventories { get; set; }
    }
}