using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pills.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AuditInterfaceImplementation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "PillsTypes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "PillsTypes",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "PillsTaken",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "PillsTaken",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "PillsTaken",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedAt",
                table: "PillsTaken",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditedBy",
                table: "PillsTaken",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "PillsTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "DeletedAt" },
                values: new object[] { new DateTime(2026, 1, 20, 16, 9, 31, 28, DateTimeKind.Local).AddTicks(9740), null });

            migrationBuilder.UpdateData(
                table: "PillsTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "DeletedAt" },
                values: new object[] { new DateTime(2026, 1, 20, 16, 9, 31, 28, DateTimeKind.Local).AddTicks(9779), null });

            migrationBuilder.UpdateData(
                table: "PillsTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "DeletedAt" },
                values: new object[] { new DateTime(2026, 1, 20, 16, 9, 31, 28, DateTimeKind.Local).AddTicks(9781), null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "PillsTypes");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PillsTaken");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PillsTaken");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "PillsTaken");

            migrationBuilder.DropColumn(
                name: "EditedAt",
                table: "PillsTaken");

            migrationBuilder.DropColumn(
                name: "EditedBy",
                table: "PillsTaken");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "PillsTypes",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValue: false);

            migrationBuilder.UpdateData(
                table: "PillsTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 20, 11, 14, 40, 233, DateTimeKind.Local).AddTicks(3878));

            migrationBuilder.UpdateData(
                table: "PillsTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 20, 11, 14, 40, 233, DateTimeKind.Local).AddTicks(3882));

            migrationBuilder.UpdateData(
                table: "PillsTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 20, 11, 14, 40, 233, DateTimeKind.Local).AddTicks(3884));
        }
    }
}
