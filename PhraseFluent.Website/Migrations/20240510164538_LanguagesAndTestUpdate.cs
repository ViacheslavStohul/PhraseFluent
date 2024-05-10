using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace PhraseFluent.API.Migrations
{
    /// <inheritdoc />
    public partial class LanguagesAndTestUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerAttempts_Cards_CardId",
                table: "AnswerAttempts");

            migrationBuilder.DropForeignKey(
                name: "FK_AnswerOptions_Cards_CardId",
                table: "AnswerOptions");

            migrationBuilder.AddColumn<string>(
                name: "NormalizedUsername",
                table: "Users",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Tests",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Tests",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Tests",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LanguageId",
                table: "Tests",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedTitle",
                table: "Tests",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Question",
                table: "Cards",
                type: "varchar(3000)",
                maxLength: 3000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<long>(
                name: "CardId",
                table: "AnswerOptions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "CompleteInitializers",
                columns: table => new
                {
                    CompleteInitializerId = table.Column<Guid>(type: "char(36)", nullable: false),
                    CompleteInitializerName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompleteInitializers", x => x.CompleteInitializerId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    NormalizedTitle = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    NativeName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    NormalizedNativeName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    LanguageCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    Uuid = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_LanguageId",
                table: "Tests",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerAttempts_AnswerOptions_CardId",
                table: "AnswerAttempts",
                column: "CardId",
                principalTable: "AnswerOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerOptions_Cards_CardId",
                table: "AnswerOptions",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_Languages_LanguageId",
                table: "Tests",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerAttempts_AnswerOptions_CardId",
                table: "AnswerAttempts");

            migrationBuilder.DropForeignKey(
                name: "FK_AnswerOptions_Cards_CardId",
                table: "AnswerOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Tests_Languages_LanguageId",
                table: "Tests");

            migrationBuilder.DropTable(
                name: "CompleteInitializers");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropIndex(
                name: "IX_Tests_LanguageId",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "NormalizedUsername",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "NormalizedTitle",
                table: "Tests");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Tests",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Tests",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Question",
                table: "Cards",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(3000)",
                oldMaxLength: 3000);

            migrationBuilder.AlterColumn<long>(
                name: "CardId",
                table: "AnswerOptions",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerAttempts_Cards_CardId",
                table: "AnswerAttempts",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerOptions_Cards_CardId",
                table: "AnswerOptions",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id");
        }
    }
}
