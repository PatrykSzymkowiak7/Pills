using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pills.Migrations
{
    /// <inheritdoc />
    public partial class DeleteMultiplePerDayAllowedProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MultiplePerDayAllowed",
                table: "PillsTypes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MultiplePerDayAllowed",
                table: "PillsTypes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "PillsTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "MultiplePerDayAllowed",
                value: false);

            migrationBuilder.UpdateData(
                table: "PillsTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "MultiplePerDayAllowed",
                value: false);

            migrationBuilder.UpdateData(
                table: "PillsTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "MultiplePerDayAllowed",
                value: true);
        }
    }
}
