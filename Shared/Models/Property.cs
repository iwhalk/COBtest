﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using SharedLibrary.Models;

namespace SharedLibrary.Models
{
    public partial class Property
    {
        public Property()
        {
            BlobsInventories = new HashSet<BlobsInventory>();
            Inventories = new HashSet<Inventory>();
            ReceptionCertificates = new HashSet<ReceptionCertificate>();
        }

        [Key]
        [Column("ID_Property")]
        public int IdProperty { get; set; }
        [Required]
        [StringLength(30)]
        [Unicode(false)]
        public string PropertyName { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Street { get; set; }
        [Required]
        [StringLength(30)]
        [Unicode(false)]
        public string Colony { get; set; }
        [Required]
        [StringLength(30)]
        [Unicode(false)]
        public string Delegation { get; set; }
        [Column("CP")]
        public string Cp { get; set; }
        public int NumberOfRooms { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        public decimal Size { get; set; }
        public bool Active { get; set; }
        [Column("ID_Lessor")]
        public int IdLessor { get; set; }
        [Column("ID_PropertyType")]
        public int IdPropertyType { get; set; }

        [ForeignKey("IdLessor")]
        [InverseProperty("Properties")]
        public virtual Lessor IdLessorNavigation { get; set; }
        [ForeignKey("IdPropertyType")]
        [InverseProperty("Properties")]
        public virtual PropertyType IdPropertyTypeNavigation { get; set; }
        [InverseProperty("IdPropertyNavigation")]
        public virtual ICollection<BlobsInventory> BlobsInventories { get; set; }
        [InverseProperty("IdPropertyNavigation")]
        public virtual ICollection<Inventory> Inventories { get; set; }
        [InverseProperty("IdPropertyNavigation")]
        public virtual ICollection<ReceptionCertificate> ReceptionCertificates { get; set; }
    }
}