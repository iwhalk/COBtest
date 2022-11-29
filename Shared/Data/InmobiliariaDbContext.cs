﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SharedLibrary.Models;

namespace SharedLibrary.Data
{
    public partial class InmobiliariaDbContext : DbContext
    {
        public InmobiliariaDbContext()
        {
        }

        public InmobiliariaDbContext(DbContextOptions<InmobiliariaDbContext> options)
            : base(options)
        {
        }

        //public virtual DbSet<Area> Areas { get; set; }
        //public virtual DbSet<AreaService> AreaServices { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Blob> Blobs { get; set; }
        public virtual DbSet<BlobsInventory> BlobsInventories { get; set; }
        //public virtual DbSet<Description> Descriptions { get; set; }
        //public virtual DbSet<Feature> Features { get; set; }
        //public virtual DbSet<Inventory> Inventories { get; set; }
        //public virtual DbSet<Lessor> Lessors { get; set; }
        //public virtual DbSet<Module> Modules { get; set; }
        //public virtual DbSet<Property> Properties { get; set; }
        //public virtual DbSet<PropertyType> PropertyTypes { get; set; }
        public virtual DbSet<ReceptionCertificate> ReceptionCertificates { get; set; }
        //public virtual DbSet<Service> Services { get; set; }
        //public virtual DbSet<Tenant> Tenants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Area>(entity =>
            //{
            //    entity.HasKey(e => e.IdArea)
            //        .HasName("PK__Areas__42A5C44CA684BD2C");
            //});

            //modelBuilder.Entity<AreaService>(entity =>
            //{
            //    entity.HasKey(e => new { e.IdArea, e.IdService, e.Id });

            //    entity.Property(e => e.Id).ValueGeneratedOnAdd();

            //    entity.HasOne(d => d.IdAreaNavigation)
            //        .WithMany(p => p.AreaServices)
            //        .HasForeignKey(d => d.IdArea)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_AreaServices_Areas");

            //    entity.HasOne(d => d.IdServiceNavigation)
            //        .WithMany(p => p.AreaServices)
            //        .HasForeignKey(d => d.IdService)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_AreaServices_Services");
            //});

            modelBuilder.Entity<Blob>(entity =>
            {
                entity.HasKey(e => e.IdBlobs)
                    .HasName("PK__Blobs__DED4FFECE2BD9BCB");
            });

            //modelBuilder.Entity<BlobsInventory>(entity =>
            //{
            //    entity.HasKey(e => e.IdBlobsInventory)
            //        .HasName("PK__BlobsInv__1A3C40D3748599FF");

            //    entity.HasOne(d => d.IdBlobsNavigation)
            //        .WithMany(p => p.BlobsInventories)
            //        .HasForeignKey(d => d.IdBlobs)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_BlobsInventories_ID_Blobs");

            //    entity.HasOne(d => d.IdInventoryNavigation)
            //        .WithMany(p => p.BlobsInventories)
            //        .HasForeignKey(d => d.IdInventory)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_BlobsInventories_ID_Inventory");

            //    entity.HasOne(d => d.IdPropertyNavigation)
            //        .WithMany(p => p.BlobsInventories)
            //        .HasForeignKey(d => d.IdProperty)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_BlobsInventories_ID_Property");
            //});

            //modelBuilder.Entity<Description>(entity =>
            //{
            //    entity.HasKey(e => e.IdDescription)
            //        .HasName("PK__Descript__145EFCBCDBFCDD5E");

            //    entity.HasOne(d => d.IdFeatureNavigation)
            //        .WithMany(p => p.Descriptions)
            //        .HasForeignKey(d => d.IdFeature)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_Descriptions_ID_Feature");
            //});

            //modelBuilder.Entity<Feature>(entity =>
            //{
            //    entity.HasKey(e => e.IdFeature)
            //        .HasName("PK__Features__F3A163F1A43CEEE9");

            //    entity.HasOne(d => d.IdServiceNavigation)
            //        .WithMany(p => p.Features)
            //        .HasForeignKey(d => d.IdService)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_Features_ID_Service");
            //});

            //modelBuilder.Entity<Inventory>(entity =>
            //{
            //    entity.HasKey(e => e.IdInventory)
            //        .HasName("PK__Inventor__2210F49E10265952");

            //    entity.HasOne(d => d.IdAreaNavigation)
            //        .WithMany(p => p.Inventories)
            //        .HasForeignKey(d => d.IdArea)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_Inventory_ID_Area");

            //    entity.HasOne(d => d.IdDescriptionNavigation)
            //        .WithMany(p => p.Inventories)
            //        .HasForeignKey(d => d.IdDescription)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_Inventory_ID_Description");

            //    entity.HasOne(d => d.IdFeatureNavigation)
            //        .WithMany(p => p.Inventories)
            //        .HasForeignKey(d => d.IdFeature)
            //        .HasConstraintName("FK_Inventories_Features");

            //    entity.HasOne(d => d.IdPropertyNavigation)
            //        .WithMany(p => p.Inventories)
            //        .HasForeignKey(d => d.IdProperty)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_Inventory_ID_Property");

            //    entity.HasOne(d => d.IdReceptionCertificateNavigation)
            //        .WithMany(p => p.Inventories)
            //        .HasForeignKey(d => d.IdReceptionCertificate)
            //        .HasConstraintName("FK_INVENTORIES_ID_ReceptionCertificate");

            //    entity.HasOne(d => d.IdServiceNavigation)
            //        .WithMany(p => p.Inventories)
            //        .HasForeignKey(d => d.IdService)
            //        .HasConstraintName("FK_Inventories_Services");
            //});

            //modelBuilder.Entity<Lessor>(entity =>
            //{
            //    entity.HasKey(e => e.IdLessor)
            //        .HasName("PK__Lessors__67381F3FADDB6F38");
            //});

            //modelBuilder.Entity<Property>(entity =>
            //{
            //    entity.HasKey(e => e.IdProperty)
            //        .HasName("PK__Properti__4C63EAA03C35CB01");

            //    entity.HasOne(d => d.IdLessorNavigation)
            //        .WithMany(p => p.Properties)
            //        .HasForeignKey(d => d.IdLessor)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_Property_ID_Lessor");

            //    entity.HasOne(d => d.IdPropertyTypeNavigation)
            //        .WithMany(p => p.Properties)
            //        .HasForeignKey(d => d.IdPropertyType)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_Property_ID_PropertyType");

            //    entity.HasMany(d => d.IdAreas)
            //        .WithMany(p => p.IdProperties)
            //        .UsingEntity<Dictionary<string, object>>(
            //            "PropertyArea",
            //            l => l.HasOne<Area>().WithMany().HasForeignKey("IdArea").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_PropertyAreas_Areas"),
            //            r => r.HasOne<Property>().WithMany().HasForeignKey("IdProperty").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_PropertyAreas_Properties"),
            //            j =>
            //            {
            //                j.HasKey("IdProperty", "IdArea");

            //                j.ToTable("PropertyAreas");

            //                j.IndexerProperty<int>("IdProperty").HasColumnName("ID_Property");

            //                j.IndexerProperty<int>("IdArea").HasColumnName("ID_Area");
            //            });
            //});

            //modelBuilder.Entity<PropertyType>(entity =>
            //{
            //    entity.HasKey(e => e.IdPropertyType)
            //        .HasName("PK__Property__EC791A66D11D7F33");
            //});

            //modelBuilder.Entity<ReceptionCertificate>(entity =>
            //{
            //    entity.HasKey(e => e.IdReceptionCertificate)
            //        .HasName("PK__Receptio__EA57197C45E9F91C");

            //    entity.HasOne(d => d.IdAgentNavigation)
            //        .WithMany(p => p.ReceptionCertificates)
            //        .HasForeignKey(d => d.IdAgent)
            //        .HasConstraintName("FK_ReceptionCertificates_ID_Agent");

            //    entity.HasOne(d => d.IdPropertyNavigation)
            //        .WithMany(p => p.ReceptionCertificates)
            //        .HasForeignKey(d => d.IdProperty)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_ReceptionCertificates_ID_Property");

            //    entity.HasOne(d => d.IdTenantNavigation)
            //        .WithMany(p => p.ReceptionCertificates)
            //        .HasForeignKey(d => d.IdTenant)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_ReceptionCertificates_ID_Tenant");
            //});

            //modelBuilder.Entity<Service>(entity =>
            //{
            //    entity.HasKey(e => e.IdService)
            //        .HasName("PK__Services__8C3D4AEF226DDD96");
            //});

            //modelBuilder.Entity<Tenant>(entity =>
            //{
            //    entity.HasKey(e => e.IdTenant)
            //        .HasName("PK__Tenants__609A421865AB7F36");
            //});

            //OnModelCreatingGeneratedProcedures(modelBuilder);
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}