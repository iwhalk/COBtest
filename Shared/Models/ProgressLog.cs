﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SharedLibrary.Models
{
    public partial class ProgressLog
    {
        [Key]
        [Column("ID_ProgressLog")]
        public int IdProgressLog { get; set; }
        [Column("ID_ProgressReport")]
        public int IdProgressReport { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateCreated { get; set; }
        [Column("ID_Status")]
        public int? IdStatus { get; set; }
        [StringLength(5)]
        [Unicode(false)]
        public string Pieces { get; set; }
        [StringLength(150)]
        [Unicode(false)]
        public string Observation { get; set; }
        [Column("ID_Supervisor")]
        [StringLength(450)]
        public string IdSupervisor { get; set; }

        [ForeignKey("IdSupervisor")]
        [InverseProperty("ProgressLogs")]
        public virtual AspNetUser IdSupervisorNavigation { get; set; }
    }
}