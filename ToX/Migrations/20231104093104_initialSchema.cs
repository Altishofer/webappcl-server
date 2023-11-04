using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ToX.Migrations
{
    /// <inheritdoc />
    public partial class initialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "answer",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    roundid = table.Column<long>(type: "bigint", nullable: false),
                    quizid = table.Column<long>(type: "bigint", nullable: false),
                    playername = table.Column<string>(type: "text", nullable: false),
                    subtractions = table.Column<List<string>>(type: "text[]", nullable: false),
                    additions = table.Column<List<string>>(type: "text[]", nullable: false),
                    answertarget = table.Column<List<string>>(type: "text[]", nullable: false),
                    distance = table.Column<double>(type: "double precision", nullable: false),
                    points = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_answer", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "host",
                columns: table => new
                {
                    hostid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hostname = table.Column<string>(type: "text", nullable: false),
                    hostpassword = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_host", x => x.hostid);
                });

            migrationBuilder.CreateTable(
                name: "player",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    playername = table.Column<string>(type: "text", nullable: false),
                    quizid = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "quiz",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hostid = table.Column<long>(type: "bigint", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quiz", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "round",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    quizid = table.Column<long>(type: "bigint", nullable: false),
                    roundtarget = table.Column<string>(type: "text", nullable: false),
                    roundtargetvector = table.Column<float[]>(type: "real[]", nullable: false),
                    forbiddenwords = table.Column<List<string>>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_round", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "wordvector",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    wordornull = table.Column<string>(type: "text", nullable: false),
                    numericvector = table.Column<float[]>(type: "real[]", nullable: false),
                    metriclength = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wordvector", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_answer_id",
                table: "answer",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_host_hostid",
                table: "host",
                column: "hostid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_player_id",
                table: "player",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_quiz_id",
                table: "quiz",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_round_id",
                table: "round",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_wordvector_wordornull",
                table: "wordvector",
                column: "wordornull",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "answer");

            migrationBuilder.DropTable(
                name: "host");

            migrationBuilder.DropTable(
                name: "player");

            migrationBuilder.DropTable(
                name: "quiz");

            migrationBuilder.DropTable(
                name: "round");

            migrationBuilder.DropTable(
                name: "wordvector");
        }
    }
}
