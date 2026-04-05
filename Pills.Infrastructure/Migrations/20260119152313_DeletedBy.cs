using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pills.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeletedBy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "PillsTypes",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "PillsTaken",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "PillsTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "DeletedBy" },
                values: new object[] { new DateTime(2026, 1, 19, 16, 23, 13, 327, DateTimeKind.Local).AddTicks(2830), "System" });

            migrationBuilder.UpdateData(
                table: "PillsTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "DeletedBy" },
                values: new object[] { new DateTime(2026, 1, 19, 16, 23, 13, 327, DateTimeKind.Local).AddTicks(2833), "System" });

            migrationBuilder.UpdateData(
                table: "PillsTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "DeletedBy" },
                values: new object[] { new DateTime(2026, 1, 19, 16, 23, 13, 327, DateTimeKind.Local).AddTicks(2836), "System" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "PillsTypes");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "PillsTaken");

            migrationBuilder.UpdateData(
                table: "PillsTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 18, 12, 10, 3, 474, DateTimeKind.Local).AddTicks(9978));

            migrationBuilder.UpdateData(
                table: "PillsTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 18, 12, 10, 3, 475, DateTimeKind.Local).AddTicks(17));

            migrationBuilder.UpdateData(
                table: "PillsTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 18, 12, 10, 3, 475, DateTimeKind.Local).AddTicks(19));
        }
    }
}
