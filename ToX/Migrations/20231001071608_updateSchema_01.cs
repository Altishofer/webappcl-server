using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToX.Migrations
{
    /// <inheritdoc />
    public partial class updateSchema_01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_user_token",
                table: "user");

            migrationBuilder.DropColumn(
                name: "token",
                table: "user");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "token",
                table: "user",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_user_token",
                table: "user",
                column: "token",
                unique: true);
        }
    }
}
