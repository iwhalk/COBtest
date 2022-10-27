﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Shared.Models
{
    public partial class LogRole
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UpdatedDate { get; set; }
        [Column("ID_User")]
        [StringLength(450)]
        public string IdUser { get; set; }
        [Required]
        [StringLength(450)]
        public string AspNetRolesId { get; set; }
        [Required]
        [StringLength(30)]
        [Unicode(false)]
        public string TypeAction { get; set; }
        [StringLength(150)]
        [Unicode(false)]
        public string OldNameRol { get; set; }
        [StringLength(150)]
        [Unicode(false)]
        public string NewNameRol { get; set; }
        public bool Active { get; set; }
    }
}