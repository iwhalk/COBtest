﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Shared.Models
{
    [Index("IdTransaction", Name = "IX_TransactionsDate")]
    public partial class Transaction
    {
        [Key]
        [Column("ID_Transaction")]
        [StringLength(30)]
        public string IdTransaction { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime TransactionDate { get; set; }
        [Column("Poste_Number")]
        public int? PosteNumber { get; set; }
        [Column("Transaction_Number")]
        public int? TransactionNumber { get; set; }
        [Column("Indice_Suite")]
        public int? IndiceSuite { get; set; }
        [Column("Entry_Date")]
        [StringLength(14)]
        public string EntryDate { get; set; }
        [Column("Rate_Table_Code")]
        [StringLength(5)]
        public string RateTableCode { get; set; }
        [Column("ISO_Content")]
        [StringLength(40)]
        public string IsoContent { get; set; }
        [Column("Payment_Method")]
        [StringLength(5)]
        public string PaymentMethod { get; set; }
        [Column("Transaction_CPT1")]
        [StringLength(6)]
        public string TransactionCpt1 { get; set; }
        [Column("Date_Debut_Poste", TypeName = "datetime")]
        public DateTime? DateDebutPoste { get; set; }
        [Column("Violation_Number")]
        [StringLength(16)]
        public string ViolationNumber { get; set; }
        [Column("Vehicle_LPN")]
        [StringLength(20)]
        public string VehicleLpn { get; set; }
        [Column("Event_Number")]
        public int? EventNumber { get; set; }
        [Column("Operation_Code")]
        [StringLength(2)]
        public string OperationCode { get; set; }
        [Column("FOLIO_ECT")]
        public int? FolioEct { get; set; }
        [Column("ID_OBS_TT")]
        [StringLength(5)]
        public string IdObsTt { get; set; }
        [Column("ID_Shift")]
        public int? IdShift { get; set; }
        [Column("ID_Class")]
        public int? IdClass { get; set; }
        [Column("ID_Class2")]
        public int? IdClass2 { get; set; }
        [Column("ID_Class3")]
        public int? IdClass3 { get; set; }
        [Column("ID_User")]
        public int IdUser { get; set; }
        [Required]
        [Column("ID_VehicleSituation")]
        [StringLength(7)]
        public string IdVehicleSituation { get; set; }
        [Required]
        [Column("ID_PaymentSituation")]
        [StringLength(7)]
        public string IdPaymentSituation { get; set; }
        [Required]
        [Column("ID_StepSituation")]
        [StringLength(7)]
        public string IdStepSituation { get; set; }
        [Column("ID_Payment")]
        public int IdPayment { get; set; }
        [Column("ID_Catalog")]
        public int IdCatalog { get; set; }
        [Column("ID_Tariff")]
        public int? IdTariff { get; set; }
        [Required]
        [Column("ID_Events")]
        [StringLength(25)]
        public string IdEvents { get; set; }
        [Column("ID_Activity")]
        public int? IdActivity { get; set; }
        [Column("ID_Sac")]
        public int? IdSac { get; set; }
        [Column("ID_Discount")]
        public int? IdDiscount { get; set; }
        public bool? Checked { get; set; }

        [ForeignKey("IdCatalog")]
        [InverseProperty("Transactions")]
        public virtual LaneCatalog IdCatalogNavigation { get; set; }
        [ForeignKey("IdClass2")]
        [InverseProperty("TransactionIdClass2Navigation")]
        public virtual TypeClass IdClass2Navigation { get; set; }
        [ForeignKey("IdClass")]
        [InverseProperty("TransactionIdClassNavigations")]
        public virtual TypeClass IdClassNavigation { get; set; }
        [ForeignKey("IdPayment")]
        [InverseProperty("Transactions")]
        public virtual TypePayment IdPaymentNavigation { get; set; }
        [ForeignKey("IdTariff")]
        [InverseProperty("Transactions")]
        public virtual Tariff IdTariffNavigation { get; set; }
        //[ForeignKey("IdUser")]
        //[InverseProperty("Transactions")]
        //public virtual UsersOpe IdUserNavigation { get; set; }
    }
}