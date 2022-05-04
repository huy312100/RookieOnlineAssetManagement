using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RookieOnlineAssetManagement.Configurations;
using RookieOnlineAssetManagement.Entities;
using RookieOnlineAssetManagement.Extensions;
using System;

namespace RookieOnlineAssetManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure using  fluent  api
            modelBuilder.ApplyConfiguration(new AssetConfiguration());
            modelBuilder.ApplyConfiguration(new AssignmentConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new LocationConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ReturnAssetConfiguration());

            // config identity
            modelBuilder.Entity<User>(b =>
            {
                // Each User can have many UserClaims
                b.HasMany<IdentityUserClaim<string>>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();

                // Each User can have many UserLogins
                b.HasMany<IdentityUserLogin<string>>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();

                // Each User can have many UserTokens
                b.HasMany<IdentityUserToken<string>>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();

                // Each User can have many entries in the UserRole join table
                b.HasMany<IdentityUserRole<string>>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
            });

            modelBuilder.Entity<IdentityUserClaim<string>>(b =>
            {
                // Primary key
                b.HasKey(uc => uc.Id);

                // Maps to the AspNetUserClaims table
                b.ToTable("UserClaims");
            });

            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins").HasKey(x => x.UserId);

            modelBuilder.Entity<IdentityUserToken<string>>(b =>
            {
                // Composite primary key consisting of the UserId, LoginProvider and Name
                b.HasKey(x => x.UserId);

                // Maps to the AspNetUserTokens table
                b.ToTable("UserTokens");
            });

            modelBuilder.Entity<Role>(b =>
            {
                // Each Role can have many entries in the UserRole join table
                b.HasMany<IdentityUserRole<string>>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();

                // Each Role can have many associated RoleClaims
                b.HasMany<IdentityRoleClaim<string>>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();
            });

            modelBuilder.Entity<IdentityRoleClaim<string>>(b =>
            {
                // Primary key
                b.HasKey(rc => rc.Id);

                // Maps to the AspNetRoleClaims table
                b.ToTable("RoleClaims");
            });

            modelBuilder.Entity<IdentityUserRole<string>>(b =>
            {
                // Primary key
                b.HasKey(r => new { r.UserId, r.RoleId });

                // Maps to the AspNetUserRoles table
                b.ToTable("UserRoles");
            });

            // seeding data
            modelBuilder.Seed();
        }

        public DbSet<Asset> Assets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<ReturnAsset> ReturnAssets { get; set; }
    }
}