using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToX.Migrations
{
    /// <inheritdoc />
    public partial class overwriteSchema_14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "playerid",
                table: "answer");

            migrationBuilder.AddColumn<string>(
                name: "additions",
                table: "answer",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "playername",
                table: "answer",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "subtractions",
                table: "answer",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "additions",
                table: "answer");

            migrationBuilder.DropColumn(
                name: "playername",
                table: "answer");

            migrationBuilder.DropColumn(
                name: "subtractions",
                table: "answer");

            migrationBuilder.AddColumn<long>(
                name: "playerid",
                table: "answer",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
