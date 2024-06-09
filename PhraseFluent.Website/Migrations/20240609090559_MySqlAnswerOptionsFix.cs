using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhraseFluent.API.Migrations
{
    /// <inheritdoc />
    public partial class MySqlAnswerOptionsFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCorrect",
                table: "AnswerOptions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCorrect",
                table: "AnswerOptions");
        }
    }
}
