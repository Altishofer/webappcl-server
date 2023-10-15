using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToX.Migrations
{
    /// <inheritdoc />
    public partial class overwriteSchema_08 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "title",
                table: "quiz",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "title",
                table: "quiz");
        }
    }
}
