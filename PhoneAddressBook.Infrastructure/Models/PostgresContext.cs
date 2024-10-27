using Microsoft.EntityFrameworkCore;
using PhoneAddressBook.API.DTOs;

namespace PhoneAddressBook.Infrastructure.Models;

public partial class PostgresContext : DbContext
{
    public PostgresContext()
    {
    }

    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Person> Persons { get; set; }

    public virtual DbSet<Phonenumber> Phonenumbers { get; set; }

    public virtual DbSet<PersonAddressPhoneDto> PersonAddressPhoneDtos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

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
            entity.Property(e => e.Type).HasConversion<int>() 
            .IsRequired();
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
                .HasMaxLength(50)
                .HasColumnName("phonenumber");

            entity.HasOne(d => d.Address).WithMany(p => p.Phonenumbers)
                .HasForeignKey(d => d.Addressid)
                .HasConstraintName("phonenumbers_addressid_fkey");
        });

        modelBuilder.Entity<PersonAddressPhoneDto>(entity =>
        {
            entity.HasNoKey();
            entity.ToView(null); 
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
