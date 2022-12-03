﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SharedLibrary.Models
{
    public partial class Apartment
    {
        public Apartment()
        {
            ProgressReports = new HashSet<ProgressReport>();
        }

        [Key]
        [Column("ID_Apartment")]
        public int IdApartment { get; set; }
        [StringLength(30)]
        [Unicode(false)]
        public string ApartmentNumber { get; set; }
        [StringLength(15)]
        [Unicode(false)]
        public string Floor { get; set; }

        [InverseProperty("IdApartmentNavigation")]
        public virtual ICollection<ProgressReport> ProgressReports { get; set; }
    }
}