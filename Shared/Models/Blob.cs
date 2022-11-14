﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Shared.Models
{
    public partial class Blob
    {
        public Blob()
        {
            BlobsInventories = new HashSet<BlobsInventory>();
        }

        [Key]
        [Column("ID_Blobs")]
        public int IdBlobs { get; set; }
        [Required]
        [StringLength(20)]
        [Unicode(false)]
        public string ContentType { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateCreated { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateModified { get; set; }
        [Required]
        [StringLength(20)]
        [Unicode(false)]
        public string BlodName { get; set; }
        [Required]
        [StringLength(20)]
        [Unicode(false)]
        public string BlodTypeId { get; set; }
        [Required]
        [StringLength(20)]
        [Unicode(false)]
        public string ContainerName { get; set; }
        public bool IsPrivate { get; set; }
        [Required]
        [Unicode(false)]
        public string Uri { get; set; }
        [Required]
        [StringLength(20)]
        [Unicode(false)]
        public string BlobSize { get; set; }

        [InverseProperty("IdBlobsNavigation")]
        public virtual ICollection<BlobsInventory> BlobsInventories { get; set; }
    }
}