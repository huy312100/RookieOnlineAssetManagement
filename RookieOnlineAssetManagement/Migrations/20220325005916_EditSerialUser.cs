using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RookieOnlineAssetManagement.Migrations
{
    public partial class EditSerialUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                columns: new[] { "ConcurrencyStamp", "JoinedDate", "PasswordHash" },
                values: new object[] { "809a3514-6382-4cae-b249-3d6f854a0039", new DateTime(2022, 3, 25, 7, 59, 15, 706, DateTimeKind.Local).AddTicks(9861), "AQAAAAEAACcQAAAAEDqvsI274lAsdoblsuggJwCA4EQtgqNGQClkNDwymWtz0tyGV/fQWHJ/AMKv86SHoQ==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "EBEFE264-FE9C-4D25-B639-C7A554D43562",
                columns: new[] { "ConcurrencyStamp", "JoinedDate", "PasswordHash" },
                values: new object[] { "c08f8191-8ae6-480c-8359-e1aafd5aef61", new DateTime(2022, 3, 25, 7, 59, 15, 700, DateTimeKind.Local).AddTicks(4677), "AQAAAAEAACcQAAAAEBizzQmcCkKKn4a8/Aloou1hNq0vVQxLp0UcBrNhFS2Ul5iZUy3SUvFzncusjy3Ixg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "354C4368-A6DC-43FA-8627-506C21DEF50E",
                column: "ConcurrencyStamp",
                value: "7b3568ee-c8df-4413-8f69-b55bac187562");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "4DB16EF4-4E54-46EA-B7CB-34892D60DF33",
                column: "ConcurrencyStamp",
                value: "7e2bb5a8-ddbc-4dbb-9762-a6ba9b701e6d");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "369364B3-0EBA-40E3-B72B-ED8CB3424B2F",
                columns: new[] { "ConcurrencyStamp", "JoinedDate", "PasswordHash" },
                values: new object[] { "ccb09067-3a83-4afc-9b91-c31694f4ded1", new DateTime(2022, 3, 24, 15, 16, 14, 648, DateTimeKind.Local).AddTicks(682), "AQAAAAEAACcQAAAAELBbsRQV6D6qE9YrFsp+Jz6W66QBJFSburAn7IIKKrFAUpCpYt/hzNtzrDSvOmU3xw==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "EBEFE264-FE9C-4D25-B639-C7A554D43562",
                columns: new[] { "ConcurrencyStamp", "JoinedDate", "PasswordHash" },
                values: new object[] { "6454e11f-bbde-4f57-bbb9-df5ef105d701", new DateTime(2022, 3, 24, 15, 16, 14, 641, DateTimeKind.Local).AddTicks(7974), "AQAAAAEAACcQAAAAELRYMHtqpbKDtjyU8I3z2dt3uF22La+21MjtkIAMYWn8r8i3r0W7rUXotkvYuqFLNg==" });
        }
    }
}
