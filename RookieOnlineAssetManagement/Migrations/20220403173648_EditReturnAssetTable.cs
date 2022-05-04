using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RookieOnlineAssetManagement.Migrations
{
    public partial class EditReturnAssetTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AssignedDate",
                table: "ReturnAssets",
                newName: "ReturnedDate");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "354C4368-A6DC-43FA-8627-506C21DEF50E",
                column: "ConcurrencyStamp",
                value: "20a4553b-8e0f-4bae-b557-db3fea590faf");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "4DB16EF4-4E54-46EA-B7CB-34892D60DF33",
                column: "ConcurrencyStamp",
                value: "b717bee8-ce56-4bc7-9c7b-f8f35fa2f7a7");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "369364B3-0EBA-40E3-B72B-ED8CB3424B2F",
                columns: new[] { "ConcurrencyStamp", "JoinedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "dad412ec-8d14-43cc-a958-e99b6609520d", new DateTime(2022, 4, 4, 0, 36, 47, 634, DateTimeKind.Local).AddTicks(8140), "AQAAAAEAACcQAAAAEJHAElF6oZsaWSE24lGhJt4yF5WD1zAtxZjka1LO4JK/fEWffZhlq+eNuZLt6HPapw==", "25d09460-4027-4f70-9fa2-6c6f1d5dec7b" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "EBEFE264-FE9C-4D25-B639-C7A554D43562",
                columns: new[] { "ConcurrencyStamp", "JoinedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a55a70b9-e4c2-4fea-8c1d-a9c9edb85b6e", new DateTime(2022, 4, 4, 0, 36, 47, 628, DateTimeKind.Local).AddTicks(3899), "AQAAAAEAACcQAAAAEIhlF/cjWN1/8H/8nI8hl9ISvvVwiMEF7LX+WJtxvAvtSEN8dF+GzcPSy+Uq+CuRPg==", "1c905b56-aa58-4926-b13d-e6cffbafec2d" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReturnedDate",
                table: "ReturnAssets",
                newName: "AssignedDate");

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
                columns: new[] { "ConcurrencyStamp", "JoinedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "78520ab2-299c-4e00-a7cb-a9c3711650e5", new DateTime(2022, 3, 31, 5, 48, 14, 258, DateTimeKind.Local).AddTicks(2946), "AQAAAAEAACcQAAAAED139E1nUbKlEyW4Rt3UGl/FNCKBQRrwnPnN9Xat0A6ciro6Q072LlMvrS2rEBK7fA==", "37cd6a48-a68e-4522-a538-8a309da6b3df" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "EBEFE264-FE9C-4D25-B639-C7A554D43562",
                columns: new[] { "ConcurrencyStamp", "JoinedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "936ab423-d062-4246-9e4f-fb7c311fc213", new DateTime(2022, 3, 31, 5, 48, 14, 249, DateTimeKind.Local).AddTicks(9168), "AQAAAAEAACcQAAAAEGVr+9o3hSHNRvEgBA/KViOwXzNGRxrW5HBtriEfiYgreQvDi/J61erh+9PKGNxljg==", "93110ea2-c5cf-43f0-b962-feb70746fc8a" });
        }
    }
}
