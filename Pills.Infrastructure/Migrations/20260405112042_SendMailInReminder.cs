using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pills.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SendMailInReminder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PillsTaken_AspNetUsers_UserId",
                table: "PillsTaken");

            migrationBuilder.DropIndex(
                name: "IX_PillsTaken_UserId",
                table: "PillsTaken");

            migrationBuilder.AddColumn<bool>(
                name: "SendMail",
                table: "Reminders",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "PillsTaken",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SendMail",
                table: "Reminders");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "PillsTaken",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_PillsTaken_UserId",
                table: "PillsTaken",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PillsTaken_AspNetUsers_UserId",
                table: "PillsTaken",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
