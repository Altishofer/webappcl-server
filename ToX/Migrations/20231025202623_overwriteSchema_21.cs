using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToX.Migrations
{
    /// <inheritdoc />
    public partial class overwriteSchema_21 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float[]>(
                name: "roundtargetvector",
                table: "round",
                type: "real[]",
                nullable: false,
                defaultValue: new float[0]);

            migrationBuilder.AlterColumn<List<string>>(
                name: "answertarget",
                table: "answer",
                type: "text[]",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<double>(
                name: "distance",
                table: "answer",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<long>(
                name: "points",
                table: "answer",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "roundtargetvector",
                table: "round");

            migrationBuilder.DropColumn(
                name: "distance",
                table: "answer");

            migrationBuilder.DropColumn(
                name: "points",
                table: "answer");

            migrationBuilder.AlterColumn<string>(
                name: "answertarget",
                table: "answer",
                type: "text",
                nullable: false,
                oldClrType: typeof(List<string>),
                oldType: "text[]");
        }
    }
}
