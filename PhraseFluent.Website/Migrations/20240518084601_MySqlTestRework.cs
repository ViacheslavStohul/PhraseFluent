using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhraseFluent.API.Migrations
{
    /// <inheritdoc />
    public partial class MySqlTestRework : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerAttempts_AnswerOptions_CardId",
                table: "AnswerAttempts");

            migrationBuilder.AddColumn<string>(
                name: "QuestionOrder",
                table: "TestAttempts",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "AnswerOptionId",
                table: "AnswerAttempts",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnswerAttempts_AnswerOptionId",
                table: "AnswerAttempts",
                column: "AnswerOptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerAttempts_AnswerOptions_AnswerOptionId",
                table: "AnswerAttempts",
                column: "AnswerOptionId",
                principalTable: "AnswerOptions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerAttempts_Cards_CardId",
                table: "AnswerAttempts",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerAttempts_AnswerOptions_AnswerOptionId",
                table: "AnswerAttempts");

            migrationBuilder.DropForeignKey(
                name: "FK_AnswerAttempts_Cards_CardId",
                table: "AnswerAttempts");

            migrationBuilder.DropIndex(
                name: "IX_AnswerAttempts_AnswerOptionId",
                table: "AnswerAttempts");

            migrationBuilder.DropColumn(
                name: "QuestionOrder",
                table: "TestAttempts");

            migrationBuilder.DropColumn(
                name: "AnswerOptionId",
                table: "AnswerAttempts");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerAttempts_AnswerOptions_CardId",
                table: "AnswerAttempts",
                column: "CardId",
                principalTable: "AnswerOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
