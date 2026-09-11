using IPMS.CustomerService.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace IPMS.CustomerService.Data;

public class CustomerDbContext : DbContext
{
    public CustomerDbContext(
        DbContextOptions<CustomerDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customers");

            entity.HasKey(x => x.CustomerId);

            entity.HasIndex(x => x.CustomerCode)
                  .IsUnique();

            entity.Property(x => x.CustomerCode)
                  .HasMaxLength(50)
                  .IsRequired();

            entity.Property(x => x.FirstName)
                  .HasMaxLength(100)
                  .IsRequired();

            entity.Property(x => x.LastName)
                  .HasMaxLength(100);

            entity.Property(x => x.Email)
                  .HasMaxLength(150)
                  .IsRequired();

            entity.Property(x => x.KycStatus)
                  .HasMaxLength(30)
                  .HasDefaultValue("Pending");
        });
    }
}