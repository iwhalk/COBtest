﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SharedLibrary.Models
{
    public partial class Module
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string NameModule { get; set; }
        [Required]
        [StringLength(50)]
        public string Image { get; set; }
        [Required]
        [StringLength(50)]
        public string Route { get; set; }
    }
}