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

        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Blob> Blobs { get; set; }
        public virtual DbSet<BlobsInventory> BlobsInventories { get; set; }
        public virtual DbSet<Description> Descriptions { get; set; }
        public virtual DbSet<Feature> Features { get; set; }
        public virtual DbSet<Inventory> Inventories { get; set; }
        public virtual DbSet<Lessor> Lessors { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<Property> Properties { get; set; }
        public virtual DbSet<PropertyType> PropertyTypes { get; set; }
        public virtual DbSet<ReceptionCertificate> ReceptionCertificates { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<Tenant> Tenants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Area>(entity =>
            {
                entity.HasKey(e => e.IdArea)
                    .HasName("PK__Areas__42A5C44C0FE9F5F8");

                entity.HasMany(d => d.IdServices)
                    .WithMany(p => p.IdAreas)
                    .UsingEntity<Dictionary<string, object>>(
                        "AreaService",
                        l => l.HasOne<Service>().WithMany().HasForeignKey("IdService").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_AreaServices_Services"),
                        r => r.HasOne<Area>().WithMany().HasForeignKey("IdArea").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_AreaServices_Areas"),
                        j =>
                        {
                            j.HasKey("IdArea", "IdService");

                            j.ToTable("AreaServices");

                            j.IndexerProperty<int>("IdArea").HasColumnName("ID_Area");

                            j.IndexerProperty<int>("IdService").HasColumnName("ID_Service");
                        });
            });

            modelBuilder.Entity<Blob>(entity =>
            {
                entity.HasKey(e => e.IdBlobs)
                    .HasName("PK__Blobs__DED4FFECA37A54E1");
            });

            modelBuilder.Entity<BlobsInventory>(entity =>
            {
                entity.HasKey(e => e.IdBlobsInventory)
                    .HasName("PK__BlobsInv__1A3C40D35F4BF0DF");

                entity.HasOne(d => d.IdBlobsNavigation)
                    .WithMany(p => p.BlobsInventories)
                    .HasForeignKey(d => d.IdBlobs)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BlobsInventories_ID_Blobs");

                entity.HasOne(d => d.IdInventoryNavigation)
                    .WithMany(p => p.BlobsInventories)
                    .HasForeignKey(d => d.IdInventory)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BlobsInventories_ID_Inventory");

                entity.HasOne(d => d.IdPropertyNavigation)
                    .WithMany(p => p.BlobsInventories)
                    .HasForeignKey(d => d.IdProperty)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BlobsInventories_ID_Property");
            });

            modelBuilder.Entity<Description>(entity =>
            {
                entity.HasKey(e => e.IdDescription)
                    .HasName("PK__Descript__145EFCBCD37C2D36");

                entity.HasOne(d => d.IdFeatureNavigation)
                    .WithMany(p => p.Descriptions)
                    .HasForeignKey(d => d.IdFeature)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Descriptions_ID_Feature");
            });

            modelBuilder.Entity<Feature>(entity =>
            {
                entity.HasKey(e => e.IdFeature)
                    .HasName("PK__Features__F3A163F1C20C0900");

                entity.HasOne(d => d.IdServiceNavigation)
                    .WithMany(p => p.Features)
                    .HasForeignKey(d => d.IdService)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Features_ID_Service");
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.HasKey(e => e.IdInventory)
                    .HasName("PK__Inventor__2210F49E10265952");

                entity.HasOne(d => d.IdAreaNavigation)
                    .WithMany(p => p.Inventories)
                    .HasForeignKey(d => d.IdArea)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inventory_ID_Area");

                entity.HasOne(d => d.IdDescriptionNavigation)
                    .WithMany(p => p.Inventories)
                    .HasForeignKey(d => d.IdDescription)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inventory_ID_Description");

                entity.HasOne(d => d.IdPropertyNavigation)
                    .WithMany(p => p.Inventories)
                    .HasForeignKey(d => d.IdProperty)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inventory_ID_Property");
            });

            modelBuilder.Entity<Lessor>(entity =>
            {
                entity.HasKey(e => e.IdLessor)
                    .HasName("PK__Lessor__67381F3FD281D2EE");
            });

            modelBuilder.Entity<Property>(entity =>
            {
                entity.HasKey(e => e.IdProperty)
                    .HasName("PK__Property__4C63EAA07AD6FF59");

                entity.HasOne(d => d.IdLessorNavigation)
                    .WithMany(p => p.Properties)
                    .HasForeignKey(d => d.IdLessor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Property_ID_Lessor");

                entity.HasOne(d => d.IdPropertyTypeNavigation)
                    .WithMany(p => p.Properties)
                    .HasForeignKey(d => d.IdPropertyType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Property_ID_PropertyType");

                entity.HasMany(d => d.IdAreas)
                    .WithMany(p => p.IdProperties)
                    .UsingEntity<Dictionary<string, object>>(
                        "PropertyArea",
                        l => l.HasOne<Area>().WithMany().HasForeignKey("IdArea").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_PropertyAreas_Areas"),
                        r => r.HasOne<Property>().WithMany().HasForeignKey("IdProperty").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_PropertyAreas_Properties"),
                        j =>
                        {
                            j.HasKey("IdProperty", "IdArea");

                            j.ToTable("PropertyAreas");

                            j.IndexerProperty<int>("IdProperty").HasColumnName("ID_Property");

                            j.IndexerProperty<int>("IdArea").HasColumnName("ID_Area");
                        });
            });

            modelBuilder.Entity<PropertyType>(entity =>
            {
                entity.HasKey(e => e.IdPropertyType)
                    .HasName("PK__Property__EC791A66ACFA202D");
            });

            modelBuilder.Entity<ReceptionCertificate>(entity =>
            {
                entity.HasKey(e => e.IdReceptionCertificate)
                    .HasName("PK__Actas__4EE6FB68E33B8ACA");

                entity.HasOne(d => d.IdAgentNavigation)
                    .WithMany(p => p.ReceptionCertificates)
                    .HasForeignKey(d => d.IdAgent)
                    .HasConstraintName("FK_ReceptionCertificates_ID_Agent");

                entity.HasOne(d => d.IdPropertyNavigation)
                    .WithMany(p => p.ReceptionCertificates)
                    .HasForeignKey(d => d.IdProperty)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReceptionCertificates_ID_Property");

                entity.HasOne(d => d.IdTenantNavigation)
                    .WithMany(p => p.ReceptionCertificates)
                    .HasForeignKey(d => d.IdTenant)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReceptionCertificates_ID_Tenant");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasKey(e => e.IdService)
                    .HasName("PK__Services__8C3D4AEFF7ABC97F");
            });

            modelBuilder.Entity<Tenant>(entity =>
            {
                entity.HasKey(e => e.IdTenant)
                    .HasName("PK__Tenant__609A42186A15AA67");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}