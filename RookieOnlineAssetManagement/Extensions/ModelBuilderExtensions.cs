using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RookieOnlineAssetManagement.Entities;
using System;

namespace RookieOnlineAssetManagement.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Location>().HasData(
                new Location() { Id = "hcm", LocationName = "Hồ Chí Minh" },
                new Location() { Id = "hn", LocationName = "Hà Nội" });
            // role and user

            var roleAdminId = "354C4368-A6DC-43FA-8627-506C21DEF50E";
            var roleStaffId = "4DB16EF4-4E54-46EA-B7CB-34892D60DF33";
            var adminHcmId = "EBEFE264-FE9C-4D25-B639-C7A554D43562";
            modelBuilder.Entity<Role>().HasData(
                new Role() { Id = roleAdminId, Name = "Admin", NormalizedName = "admin", Description = "Adminstrator role" },
                new Role() { Id = roleStaffId, Name = "Staff", NormalizedName = "staff", Description = "Staff role" }
                );
            var hasher = new PasswordHasher<User>();
            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    Id = adminHcmId,
                    FirstName = "Nghia",
                    LastName = "Nguyen Gia Huu",
                    Email = "nguyengiahuunghia118@gmail.com",
                    BirthDay = new DateTime(1999, 6, 8),
                    EmailConfirmed = true,
                    NormalizedEmail = "nguyengiahuunghia118@gmail.com",
                    UserName = "nghiangh",
                    PasswordHash = hasher.HashPassword(null, "nghiangh@08061999"),
                    NormalizedUserName = "nghiangh",
                    LocationId = "hcm",
                    Gender = Enums.Gender.Male,
                    JoinedDate = DateTime.Now,
                    IsNew = false
                }
                );
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { RoleId = roleAdminId, UserId = adminHcmId }
                );

            var adminHnId = "369364B3-0EBA-40E3-B72B-ED8CB3424B2F";
            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    Id = adminHnId,
                    FirstName = "Khang",
                    LastName = "Nguyen Duc",
                    Email = "nguyenduckhang@gmail.com",
                    BirthDay = new DateTime(2000, 10, 31),
                    EmailConfirmed = true,
                    NormalizedEmail = "nguyenduckhang@gmail.com",
                    UserName = "khangnd",
                    PasswordHash = hasher.HashPassword(null, "khangnd@31102000"),
                    NormalizedUserName = "khangnd",
                    LocationId = "hn",
                    Gender = Enums.Gender.Male,
                    JoinedDate = DateTime.Now,
                    IsNew = false
                });
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { RoleId = roleAdminId, UserId = adminHnId }
                );
        }
    }
}