﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Shared.Models
{
    public partial class ReceptionCertificate
    {
        [Key]
        [Column("ID_ReceptionCertificate")]
        public int IdReceptionCertificate { get; set; }
        [Column("ID_Property")]
        public int IdProperty { get; set; }
        [Column("ID_Tenant")]
        public int IdTenant { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreationDate { get; set; }
        [Required]
        [StringLength(30)]
        [Unicode(false)]
        public string ContractNumber { get; set; }
        [StringLength(150)]
        [Unicode(false)]
        public string Observation { get; set; }
        [StringLength(200)]
        [Unicode(false)]
        public string ApprovarPathLessor { get; set; }
        [StringLength(200)]
        [Unicode(false)]
        public string ApprovalPathTenant { get; set; }
        [StringLength(200)]
        [Unicode(false)]
        public string Stamp { get; set; }
        [Column("ID_Agent")]
        [StringLength(450)]
        public string IdAgent { get; set; }
        [Column("ID_TypeRecord")]
        public int IdTypeRecord { get; set; }

        [ForeignKey("IdAgent")]
        [InverseProperty("ReceptionCertificates")]
        public virtual AspNetUser IdAgentNavigation { get; set; }
        [ForeignKey("IdProperty")]
        [InverseProperty("ReceptionCertificates")]
        public virtual Property IdPropertyNavigation { get; set; }
        [ForeignKey("IdTenant")]
        [InverseProperty("ReceptionCertificates")]
        public virtual Tenant IdTenantNavigation { get; set; }
    }
}