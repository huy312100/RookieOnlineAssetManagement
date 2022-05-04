using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RookieOnlineAssetManagement.Migrations
{
    public partial class AddIsNewColumnToUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsNew",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
                columns: new[] { "ConcurrencyStamp", "JoinedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f4c4060b-fab0-4b98-ab16-b7480589e45b", new DateTime(2022, 3, 28, 13, 43, 48, 652, DateTimeKind.Local).AddTicks(188), "AQAAAAEAACcQAAAAEN/Ti+70uejPX6j6dc7EJ6dQ8TDJXRspmdNxSCbu5aeaGQTXFR0RKdwIy3u+opvFYQ==", "03852a5f-0998-45c4-9681-db6643f6a9c6" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "EBEFE264-FE9C-4D25-B639-C7A554D43562",
                columns: new[] { "ConcurrencyStamp", "JoinedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "480e16ec-2f0f-46aa-9494-1273971aca9b", new DateTime(2022, 3, 28, 13, 43, 48, 645, DateTimeKind.Local).AddTicks(5275), "AQAAAAEAACcQAAAAEDV6GZVT7Q9t25AaIC+jmpG7g0l72JJAzIJcB/Beu1JwPwhge040Z0xx0dARp6qEwA==", "11dad8fa-da4b-4f38-8885-d596a7ed2fed" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNew",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "354C4368-A6DC-43FA-8627-506C21DEF50E",
                column: "ConcurrencyStamp",
                value: "29cede71-dc42-4c29-9364-3386c2e82ba1");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "4DB16EF4-4E54-46EA-B7CB-34892D60DF33",
                column: "ConcurrencyStamp",
                value: "bc94e485-ec42-48fe-a86c-5ceb5e972966");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "369364B3-0EBA-40E3-B72B-ED8CB3424B2F",
                columns: new[] { "ConcurrencyStamp", "JoinedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "809a3514-6382-4cae-b249-3d6f854a0039", new DateTime(2022, 3, 25, 7, 59, 15, 706, DateTimeKind.Local).AddTicks(9861), "AQAAAAEAACcQAAAAEDqvsI274lAsdoblsuggJwCA4EQtgqNGQClkNDwymWtz0tyGV/fQWHJ/AMKv86SHoQ==", "" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "EBEFE264-FE9C-4D25-B639-C7A554D43562",
                columns: new[] { "ConcurrencyStamp", "JoinedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c08f8191-8ae6-480c-8359-e1aafd5aef61", new DateTime(2022, 3, 25, 7, 59, 15, 700, DateTimeKind.Local).AddTicks(4677), "AQAAAAEAACcQAAAAEBizzQmcCkKKn4a8/Aloou1hNq0vVQxLp0UcBrNhFS2Ul5iZUy3SUvFzncusjy3Ixg==", "" });
        }
    }
}
