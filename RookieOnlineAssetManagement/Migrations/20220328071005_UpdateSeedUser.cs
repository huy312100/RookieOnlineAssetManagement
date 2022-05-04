using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RookieOnlineAssetManagement.Migrations
{
    public partial class UpdateSeedUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                columns: new[] { "ConcurrencyStamp", "JoinedDate", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "13d57f3a-e7b3-490f-8509-e479e4aa6f4e", new DateTime(2022, 3, 28, 14, 10, 4, 805, DateTimeKind.Local).AddTicks(5382), "khangnd", "AQAAAAEAACcQAAAAEPFm/w8PEI81xnEApWwsjVGgh/Hy4YBYYYPVymzIPXulsEBxrs6WIW1OE3otzyHdHw==", "88f650d5-9a44-4d89-bc0f-c40e243a93a6" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "EBEFE264-FE9C-4D25-B639-C7A554D43562",
                columns: new[] { "ConcurrencyStamp", "JoinedDate", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6ee30e56-a7d2-4e1d-89f8-47f4fe9edd14", new DateTime(2022, 3, 28, 14, 10, 4, 798, DateTimeKind.Local).AddTicks(6554), "nghiangh", "AQAAAAEAACcQAAAAEKbv+jkGuGaLNcVR3qAtRe8nnjxVhRMdhdvAC5uGGqYtkXYVZN0Wjf6kFLy8VrZVDQ==", "01cbb69c-a71f-4d72-84eb-72373f467cd1" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "354C4368-A6DC-43FA-8627-506C21DEF50E",
                column: "ConcurrencyStamp",
                value: "77ad4cc2-e67d-435e-b8ab-86b2efa795ae");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "4DB16EF4-4E54-46EA-B7CB-34892D60DF33",
                column: "ConcurrencyStamp",
                value: "009081be-3939-4329-a036-6e431156339b");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "369364B3-0EBA-40E3-B72B-ED8CB3424B2F",
                columns: new[] { "ConcurrencyStamp", "JoinedDate", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f4c4060b-fab0-4b98-ab16-b7480589e45b", new DateTime(2022, 3, 28, 13, 43, 48, 652, DateTimeKind.Local).AddTicks(188), "admin", "AQAAAAEAACcQAAAAEN/Ti+70uejPX6j6dc7EJ6dQ8TDJXRspmdNxSCbu5aeaGQTXFR0RKdwIy3u+opvFYQ==", "03852a5f-0998-45c4-9681-db6643f6a9c6" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "EBEFE264-FE9C-4D25-B639-C7A554D43562",
                columns: new[] { "ConcurrencyStamp", "JoinedDate", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "480e16ec-2f0f-46aa-9494-1273971aca9b", new DateTime(2022, 3, 28, 13, 43, 48, 645, DateTimeKind.Local).AddTicks(5275), "admin", "AQAAAAEAACcQAAAAEDV6GZVT7Q9t25AaIC+jmpG7g0l72JJAzIJcB/Beu1JwPwhge040Z0xx0dARp6qEwA==", "11dad8fa-da4b-4f38-8885-d596a7ed2fed" });
        }
    }
}
