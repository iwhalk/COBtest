﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Shared.Models;

namespace Shared.Data
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

        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Lessor> Lessors { get; set; }
        public virtual DbSet<Property> Properties { get; set; }
        public virtual DbSet<PropertyType> PropertyTypes { get; set; }
        public virtual DbSet<Tenant> Tenants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
            });

            modelBuilder.Entity<PropertyType>(entity =>
            {
                entity.HasKey(e => e.IdPropertyType)
                    .HasName("PK__Property__EC791A66ACFA202D");
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