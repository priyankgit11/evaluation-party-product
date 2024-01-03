using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EvaluationPartyProduct.Models;

public partial class EvaluationDbContext : DbContext
{
    public EvaluationDbContext()
    {
    }

    public EvaluationDbContext(DbContextOptions<EvaluationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblAssignParty> TblAssignParties { get; set; }

    public virtual DbSet<TblInvoice> TblInvoices { get; set; }

    public virtual DbSet<TblInvoiceDetail> TblInvoiceDetails { get; set; }

    public virtual DbSet<TblParty> TblParties { get; set; }

    public virtual DbSet<TblProduct> TblProducts { get; set; }

    public virtual DbSet<TblProductRate> TblProductRates { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblAssignParty>(entity =>
        {
            entity.ToTable("tblAssignParty");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PartyId).HasColumnName("party_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");

            entity.HasOne(d => d.Party).WithMany(p => p.TblAssignParties)
                .HasForeignKey(d => d.PartyId)
                .HasConstraintName("FK_tblAssignParty_tblParty");

            entity.HasOne(d => d.Product).WithMany(p => p.TblAssignParties)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_tblAssignParty_tblProduct");
        });

        modelBuilder.Entity<TblInvoice>(entity =>
        {
            entity.ToTable("tblInvoice");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.PartyId).HasColumnName("party_id");

            entity.HasOne(d => d.Party).WithMany(p => p.TblInvoices)
                .HasForeignKey(d => d.PartyId)
                .HasConstraintName("FK_tblInvoice_tblParty");
        });

        modelBuilder.Entity<TblInvoiceDetail>(entity =>
        {
            entity.ToTable("tblInvoiceDetail");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Rate)
                .HasColumnType("decimal(9, 2)")
                .HasColumnName("rate");

            entity.HasOne(d => d.Invoice).WithMany(p => p.TblInvoiceDetails)
                .HasForeignKey(d => d.InvoiceId)
                .HasConstraintName("FK_tblInvoiceDetail_tblInvoice");

            entity.HasOne(d => d.Product).WithMany(p => p.TblInvoiceDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_tblInvoiceDetail_tblProduct");
        });

        modelBuilder.Entity<TblParty>(entity =>
        {
            entity.ToTable("tblParty");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PartyName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("party_name");
        });

        modelBuilder.Entity<TblProduct>(entity =>
        {
            entity.ToTable("tblProduct");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("product_name");
        });

        modelBuilder.Entity<TblProductRate>(entity =>
        {
            entity.ToTable("tblProductRate");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Rate)
                .HasColumnType("decimal(9, 2)")
                .HasColumnName("rate");

            entity.HasOne(d => d.Product).WithMany(p => p.TblProductRates)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_tblProductRate_tblProduct");
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.ToTable("tblUsers");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
