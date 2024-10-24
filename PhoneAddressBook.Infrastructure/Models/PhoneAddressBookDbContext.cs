using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PhoneAddressBook.Infrastructure.Models;

public partial class PhoneAddressBookDbContext : DbContext
{
    public PhoneAddressBookDbContext()
    {
    }

    public PhoneAddressBookDbContext(DbContextOptions<PhoneAddressBookDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Person> Persons { get; set; }

    public virtual DbSet<Phonenumber> Phonenumbers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=database-2.cp0u0o8yqdh5.eu-north-1.rds.amazonaws.com;Database=postgres;Username=postgres;Password=phoneAddressBook123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("addresses_pkey");

            entity.ToTable("addresses");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address1)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.Personid).HasColumnName("personid");
            entity.Property(e => e.Type).HasColumnName("type");

            entity.HasOne(d => d.Person).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.Personid)
                .HasConstraintName("addresses_personid_fkey");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("persons_pkey");

            entity.ToTable("persons");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Fullname)
                .HasMaxLength(255)
                .HasColumnName("fullname");
        });

        modelBuilder.Entity<Phonenumber>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("phonenumbers_pkey");

            entity.ToTable("phonenumbers");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Addressid).HasColumnName("addressid");
            entity.Property(e => e.Phonenumber1)
                .HasMaxLength(20)
                .HasColumnName("phonenumber");

            entity.HasOne(d => d.Address).WithMany(p => p.Phonenumbers)
                .HasForeignKey(d => d.Addressid)
                .HasConstraintName("phonenumbers_addressid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
