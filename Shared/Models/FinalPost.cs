﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Shared.Models
{
    public partial class FinalPost
    {
        [Key]
        [Column("ID_FinalPosts")]
        public int IdFinalPosts { get; set; }
        public int PosteNumber { get; set; }
        [Column("Date_FinalPosts", TypeName = "datetime")]
        public DateTime DateFinalPosts { get; set; }
        [Column("Date_DebutPosts", TypeName = "datetime")]
        public DateTime DateDebutPosts { get; set; }
        [Column("Initial_EventNumber")]
        public int? InitialEventNumber { get; set; }
        [Column("Final_EventNumber")]
        public int? FinalEventNumber { get; set; }
        [Column("FolioECT_NumberInitial")]
        public int? FolioEctNumberInitial { get; set; }
        [Column("FolioECT_NumberFinal")]
        public int? FolioEctNumberFinal { get; set; }
        [Column("ID_User")]
        public int IdUser { get; set; }
        [Column("ID_Catalog")]
        public int IdCatalog { get; set; }
        [Column("ID_Sac")]
        public int? IdSac { get; set; }
        [Column("Date_Reddition", TypeName = "datetime")]
        public DateTime? DateReddition { get; set; }
        public int? Sac { get; set; }
        public int? TraficTotal { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string RecetteMonnaie1 { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string RecetteMonnaie2 { get; set; }
        [Column("FinPosteCPT6")]
        public int? FinPosteCpt6 { get; set; }
        [Column("FinPosteCPT8")]
        public int? FinPosteCpt8 { get; set; }
        [Column("FinPosteCPT21")]
        public int? FinPosteCpt21 { get; set; }
        [Column("FinPosteCPT22")]
        public int? FinPosteCpt22 { get; set; }

        [ForeignKey("IdCatalog")]
        [InverseProperty("FinalPosts")]
        public virtual LaneCatalog IdCatalogNavigation { get; set; }
        [ForeignKey("IdUser")]
        [InverseProperty("FinalPosts")]
        public virtual UsersOpe IdUserNavigation { get; set; }
    }
}