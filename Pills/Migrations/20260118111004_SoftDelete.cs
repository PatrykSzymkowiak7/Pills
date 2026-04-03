using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pills.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "PillsTypes",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "PillsTypes",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedAt",
                table: "PillsTypes",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "EditedBy",
                table: "PillsTypes",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PillsTypes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PillsTaken",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "PillsTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "CreatedBy", "EditedAt", "EditedBy", "IsDeleted" },
                values: new object[] { new DateTime(2026, 1, 18, 12, 10, 3, 474, DateTimeKind.Local).AddTicks(9978), "System", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false });

            migrationBuilder.UpdateData(
                table: "PillsTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "CreatedBy", "EditedAt", "EditedBy", "IsDeleted" },
                values: new object[] { new DateTime(2026, 1, 18, 12, 10, 3, 475, DateTimeKind.Local).AddTicks(17), "System", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false });

            migrationBuilder.UpdateData(
                table: "PillsTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "CreatedBy", "EditedAt", "EditedBy", "IsDeleted" },
                values: new object[] { new DateTime(2026, 1, 18, 12, 10, 3, 475, DateTimeKind.Local).AddTicks(19), "System", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PillsTypes");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PillsTypes");

            migrationBuilder.DropColumn(
                name: "EditedAt",
                table: "PillsTypes");

            migrationBuilder.DropColumn(
                name: "EditedBy",
                table: "PillsTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PillsTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PillsTaken");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
