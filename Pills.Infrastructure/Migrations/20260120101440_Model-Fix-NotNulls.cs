using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pills.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModelFixNotNulls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedAt",
                table: "PillsTypes",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "PillsTypes",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "PillsTaken",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "PillsTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "DeletedBy", "EditedAt" },
                values: new object[] { new DateTime(2026, 1, 20, 11, 14, 40, 233, DateTimeKind.Local).AddTicks(3878), null, null });

            migrationBuilder.UpdateData(
                table: "PillsTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "DeletedBy", "EditedAt" },
                values: new object[] { new DateTime(2026, 1, 20, 11, 14, 40, 233, DateTimeKind.Local).AddTicks(3882), null, null });

            migrationBuilder.UpdateData(
                table: "PillsTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "DeletedBy", "EditedAt" },
                values: new object[] { new DateTime(2026, 1, 20, 11, 14, 40, 233, DateTimeKind.Local).AddTicks(3884), null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "EditedAt",
                table: "PillsTypes",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "PillsTypes",
                keyColumn: "DeletedBy",
                keyValue: null,
                column: "DeletedBy",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "PillsTypes",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "PillsTaken",
                keyColumn: "DeletedBy",
                keyValue: null,
                column: "DeletedBy",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "PillsTaken",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "PillsTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "DeletedBy", "EditedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 16, 23, 13, 327, DateTimeKind.Local).AddTicks(2830), "System", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "PillsTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "DeletedBy", "EditedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 16, 23, 13, 327, DateTimeKind.Local).AddTicks(2833), "System", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "PillsTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "DeletedBy", "EditedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 16, 23, 13, 327, DateTimeKind.Local).AddTicks(2836), "System", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}
