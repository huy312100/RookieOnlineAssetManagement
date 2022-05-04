using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RookieOnlineAssetManagement.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "LocationName" },
                values: new object[,]
                {
                    { "hcm", "Hồ Chí Minh" },
                    { "hn", "Hà Nội" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "354C4368-A6DC-43FA-8627-506C21DEF50E", "7b3568ee-c8df-4413-8f69-b55bac187562", "Adminstrator role", "Admin", "admin" },
                    { "4DB16EF4-4E54-46EA-B7CB-34892D60DF33", "7e2bb5a8-ddbc-4dbb-9762-a6ba9b701e6d", "Staff role", "Staff", "staff" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "BirthDay", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "Gender", "JoinedDate", "LastName", "LocationId", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UserName" },
                values: new object[] { "EBEFE264-FE9C-4D25-B639-C7A554D43562", 0, new DateTime(1999, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "6454e11f-bbde-4f57-bbb9-df5ef105d701", "nguyengiahuunghia118@gmail.com", true, "Nghia", 0, new DateTime(2022, 3, 24, 15, 16, 14, 641, DateTimeKind.Local).AddTicks(7974), "Nguyen Gia Huu", "hcm", false, null, "nguyengiahuunghia118@gmail.com", "admin", "AQAAAAEAACcQAAAAELRYMHtqpbKDtjyU8I3z2dt3uF22La+21MjtkIAMYWn8r8i3r0W7rUXotkvYuqFLNg==", null, false, "", 0, false, "nghiangh" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "BirthDay", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "Gender", "JoinedDate", "LastName", "LocationId", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UserName" },
                values: new object[] { "369364B3-0EBA-40E3-B72B-ED8CB3424B2F", 0, new DateTime(2000, 10, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "ccb09067-3a83-4afc-9b91-c31694f4ded1", "nguyenduckhang@gmail.com", true, "Khang", 0, new DateTime(2022, 3, 24, 15, 16, 14, 648, DateTimeKind.Local).AddTicks(682), "Nguyen Duc", "hn", false, null, "nguyenduckhang@gmail.com", "admin", "AQAAAAEAACcQAAAAELBbsRQV6D6qE9YrFsp+Jz6W66QBJFSburAn7IIKKrFAUpCpYt/hzNtzrDSvOmU3xw==", null, false, "", 0, false, "khangnd" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "354C4368-A6DC-43FA-8627-506C21DEF50E", "EBEFE264-FE9C-4D25-B639-C7A554D43562" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "354C4368-A6DC-43FA-8627-506C21DEF50E", "369364B3-0EBA-40E3-B72B-ED8CB3424B2F" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "4DB16EF4-4E54-46EA-B7CB-34892D60DF33");

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "354C4368-A6DC-43FA-8627-506C21DEF50E", "369364B3-0EBA-40E3-B72B-ED8CB3424B2F" });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "354C4368-A6DC-43FA-8627-506C21DEF50E", "EBEFE264-FE9C-4D25-B639-C7A554D43562" });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "354C4368-A6DC-43FA-8627-506C21DEF50E");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "369364B3-0EBA-40E3-B72B-ED8CB3424B2F");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "EBEFE264-FE9C-4D25-B639-C7A554D43562");

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: "hcm");

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: "hn");
        }
    }
}
