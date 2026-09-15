using IPMS.IdentityService.Entities;
using Microsoft.EntityFrameworkCore;

namespace IPMS.IdentityService.Data;

public class IdentityDbContext : DbContext
{
    public IdentityDbContext(
        DbContextOptions<IdentityDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Role> Roles => Set<Role>();

    public DbSet<Permission> Permissions => Set<Permission>();

    public DbSet<UserRole> UserRoles => Set<UserRole>();

    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");

            entity.HasKey(x => x.UserId);

            entity.HasIndex(x => x.UserName)
                  .IsUnique();

            entity.HasIndex(x => x.Email)
                  .IsUnique();

            entity.Property(x => x.UserName)
                  .HasMaxLength(100)
                  .IsRequired();

            entity.Property(x => x.Email)
                  .HasMaxLength(200)
                  .IsRequired();

            entity.Property(x => x.PasswordHash)
                  .HasMaxLength(500)
                  .IsRequired();
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Roles");

            entity.HasKey(x => x.RoleId);

            entity.HasIndex(x => x.RoleName)
                  .IsUnique();

            entity.Property(x => x.RoleName)
                  .HasMaxLength(100)
                  .IsRequired();
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.ToTable("Permissions");

            entity.HasKey(x => x.PermissionId);

            entity.HasIndex(x => x.PermissionCode)
                  .IsUnique();

            entity.Property(x => x.PermissionCode)
                  .HasMaxLength(100)
                  .IsRequired();
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.ToTable("UserRoles");

            entity.HasKey(x => new
            {
                x.UserId,
                x.RoleId
            });

            entity.HasOne(x => x.User)
                  .WithMany(x => x.UserRoles)
                  .HasForeignKey(x => x.UserId);

            entity.HasOne(x => x.Role)
                  .WithMany(x => x.UserRoles)
                  .HasForeignKey(x => x.RoleId);
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.ToTable("RolePermissions");

            entity.HasKey(x => new
            {
                x.RoleId,
                x.PermissionId
            });

            entity.HasOne(x => x.Role)
                  .WithMany(x => x.RolePermissions)
                  .HasForeignKey(x => x.RoleId);

            entity.HasOne(x => x.Permission)
                  .WithMany(x => x.RolePermissions)
                  .HasForeignKey(x => x.PermissionId);
        });
    }
}