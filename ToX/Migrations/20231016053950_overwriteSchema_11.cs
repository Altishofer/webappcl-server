using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToX.Migrations
{
    /// <inheritdoc />
    public partial class overwriteSchema_11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_round_wordvector_roundtargetid",
                table: "round");

            migrationBuilder.DropIndex(
                name: "IX_round_roundtargetid",
                table: "round");

            migrationBuilder.DropColumn(
                name: "roundtargetid",
                table: "round");

            migrationBuilder.AddColumn<string>(
                name: "roundtarget",
                table: "round",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "roundtarget",
                table: "round");

            migrationBuilder.AddColumn<int>(
                name: "roundtargetid",
                table: "round",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_round_roundtargetid",
                table: "round",
                column: "roundtargetid");

            migrationBuilder.AddForeignKey(
                name: "FK_round_wordvector_roundtargetid",
                table: "round",
                column: "roundtargetid",
                principalTable: "wordvector",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
