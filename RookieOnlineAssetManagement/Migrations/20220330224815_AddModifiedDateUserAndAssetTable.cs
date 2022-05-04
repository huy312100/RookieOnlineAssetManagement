using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RookieOnlineAssetManagement.Migrations
{
    public partial class AddModifiedDateUserAndAssetTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Assets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "354C4368-A6DC-43FA-8627-506C21DEF50E",
                column: "ConcurrencyStamp",
                value: "dfeaa31a-d52c-49df-be0d-f09d9051989e");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "4DB16EF4-4E54-46EA-B7CB-34892D60DF33",
                column: "ConcurrencyStamp",
                value: "e3a34b43-6c5c-483c-a194-e766b2c9f978");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "369364B3-0EBA-40E3-B72B-ED8CB3424B2F",
                columns: new[] { "ConcurrencyStamp", "Gender", "JoinedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "78520ab2-299c-4e00-a7cb-a9c3711650e5", 2, new DateTime(2022, 3, 31, 5, 48, 14, 258, DateTimeKind.Local).AddTicks(2946), "AQAAAAEAACcQAAAAED139E1nUbKlEyW4Rt3UGl/FNCKBQRrwnPnN9Xat0A6ciro6Q072LlMvrS2rEBK7fA==", "37cd6a48-a68e-4522-a538-8a309da6b3df" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "EBEFE264-FE9C-4D25-B639-C7A554D43562",
                columns: new[] { "ConcurrencyStamp", "Gender", "JoinedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "936ab423-d062-4246-9e4f-fb7c311fc213", 2, new DateTime(2022, 3, 31, 5, 48, 14, 249, DateTimeKind.Local).AddTicks(9168), "AQAAAAEAACcQAAAAEGVr+9o3hSHNRvEgBA/KViOwXzNGRxrW5HBtriEfiYgreQvDi/J61erh+9PKGNxljg==", "93110ea2-c5cf-43f0-b962-feb70746fc8a" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Assets");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "354C4368-A6DC-43FA-8627-506C21DEF50E",
                column: "ConcurrencyStamp",
                value: "6cb0cc71-7e8c-4442-aff2-c28cc65032a0");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "4DB16EF4-4E54-46EA-B7CB-34892D60DF33",
                column: "ConcurrencyStamp",
                value: "e68e315d-eb00-4d80-b267-da61f17f76ee");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "369364B3-0EBA-40E3-B72B-ED8CB3424B2F",
                columns: new[] { "ConcurrencyStamp", "Gender", "JoinedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "13d57f3a-e7b3-490f-8509-e479e4aa6f4e", 0, new DateTime(2022, 3, 28, 14, 10, 4, 805, DateTimeKind.Local).AddTicks(5382), "AQAAAAEAACcQAAAAEPFm/w8PEI81xnEApWwsjVGgh/Hy4YBYYYPVymzIPXulsEBxrs6WIW1OE3otzyHdHw==", "88f650d5-9a44-4d89-bc0f-c40e243a93a6" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "EBEFE264-FE9C-4D25-B639-C7A554D43562",
                columns: new[] { "ConcurrencyStamp", "Gender", "JoinedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6ee30e56-a7d2-4e1d-89f8-47f4fe9edd14", 0, new DateTime(2022, 3, 28, 14, 10, 4, 798, DateTimeKind.Local).AddTicks(6554), "AQAAAAEAACcQAAAAEKbv+jkGuGaLNcVR3qAtRe8nnjxVhRMdhdvAC5uGGqYtkXYVZN0Wjf6kFLy8VrZVDQ==", "01cbb69c-a71f-4d72-84eb-72373f467cd1" });
        }
    }
}
