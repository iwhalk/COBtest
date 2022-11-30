﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SharedLibrary.Models
{
    public partial class Building
    {
        [Key]
        [Column("ID_Building")]
        public int IdBuilding { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Street { get; set; }
        [Required]
        [StringLength(30)]
        [Unicode(false)]
        public string Neighborhood { get; set; }
        [Required]
        [StringLength(30)]
        [Unicode(false)]
        public string District { get; set; }
        [StringLength(10)]
        public string ZipCode { get; set; }
        public int? TotalApartments { get; set; }
    }
}