using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhraseFluent.API.Migrations
{
    /// <inheritdoc />
    public partial class MySqlTestCompletion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Completed",
                table: "TestAttempts",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TextAnswer",
                table: "AnswerAttempts",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Completed",
                table: "TestAttempts");

            migrationBuilder.DropColumn(
                name: "TextAnswer",
                table: "AnswerAttempts");
        }
    }
}
