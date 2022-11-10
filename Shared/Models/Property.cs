﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Shared.Models
{
    public partial class Property
    {
        public Property()
        {
            BlobsInventories = new HashSet<BlobsInventory>();
            Inventories = new HashSet<Inventory>();
            ReceptionCertificates = new HashSet<ReceptionCertificate>();
        }

        public int IdProperty { get; set; }
        public string PropertyName { get; set; }
        public string Street { get; set; }
        public string Colony { get; set; }
        public string Delegation { get; set; }
        [Required]
        [Column("CP")]
        [StringLength(10)]
        public string Cp { get; set; }
        public int NumberOfRooms { get; set; }
        public decimal Size { get; set; }
        public bool Active { get; set; }
        public int IdLessor { get; set; }
        public int IdPropertyType { get; set; }

        public virtual Lessor IdLessorNavigation { get; set; }
        public virtual PropertyType IdPropertyTypeNavigation { get; set; }
        public virtual ICollection<BlobsInventory> BlobsInventories { get; set; }
        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<ReceptionCertificate> ReceptionCertificates { get; set; }
    }
}