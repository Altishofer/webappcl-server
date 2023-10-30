using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToX.Migrations
{
    /// <inheritdoc />
    public partial class overwriteSchema_09 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuizId",
                table: "player",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_player_Id",
                table: "player",
                newName: "IX_player_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "player",
                newName: "QuizId");

            migrationBuilder.RenameIndex(
                name: "IX_player_id",
                table: "player",
                newName: "IX_player_Id");
        }
    }
}
