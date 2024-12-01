using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DistLearning.API.Migrations
{
    /// <inheritdoc />
    public partial class anotherfieldMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerAttempts_Cards_CardId",
                table: "AnswerAttempts");

            migrationBuilder.DropForeignKey(
                name: "FK_AnswerAttempts_TestAttempts_TestAttemptId",
                table: "AnswerAttempts");

            migrationBuilder.AddColumn<bool>(
                name: "IsAllowedText",
                table: "AnswerOptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerAttempts_Cards_CardId",
                table: "AnswerAttempts",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerAttempts_TestAttempts_TestAttemptId",
                table: "AnswerAttempts",
                column: "TestAttemptId",
                principalTable: "TestAttempts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerAttempts_Cards_CardId",
                table: "AnswerAttempts");

            migrationBuilder.DropForeignKey(
                name: "FK_AnswerAttempts_TestAttempts_TestAttemptId",
                table: "AnswerAttempts");

            migrationBuilder.DropColumn(
                name: "IsAllowedText",
                table: "AnswerOptions");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerAttempts_Cards_CardId",
                table: "AnswerAttempts",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerAttempts_TestAttempts_TestAttemptId",
                table: "AnswerAttempts",
                column: "TestAttemptId",
                principalTable: "TestAttempts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
