﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Shared.Models
{
    [Table("PropertyType")]
    public partial class PropertyType
    {
        public PropertyType()
        {
            Properties = new HashSet<Property>();
        }

        [Key]
        [Column("ID_PropertyType")]
        public int IdPropertyType { get; set; }
        [Required]
        [StringLength(30)]
        [Unicode(false)]
        public string PropertyTypeName { get; set; }

        [InverseProperty("IdPropertyTypeNavigation")]
        public virtual ICollection<Property> Properties { get; set; }
    }
}