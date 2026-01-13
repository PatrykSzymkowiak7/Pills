using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pills.Migrations
{
    /// <inheritdoc />
    public partial class PillsTypesMaxAllowed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxAllowed",
                table: "PillsTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "PillsTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "MaxAllowed",
                value: 1);

            migrationBuilder.UpdateData(
                table: "PillsTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "MaxAllowed",
                value: 1);

            migrationBuilder.UpdateData(
                table: "PillsTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "MaxAllowed", "MultiplePerDayAllowed" },
                values: new object[] { 5, true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxAllowed",
                table: "PillsTypes");

            migrationBuilder.UpdateData(
                table: "PillsTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "MultiplePerDayAllowed",
                value: false);
        }
    }
}
