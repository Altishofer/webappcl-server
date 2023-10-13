using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ToX.Migrations
{
    /// <inheritdoc />
    public partial class overwriteSchema_05 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_host_hostName",
                table: "host");

            migrationBuilder.RenameColumn(
                name: "WordOrNull",
                table: "wordvector",
                newName: "wordornull");

            migrationBuilder.RenameColumn(
                name: "NumericVector",
                table: "wordvector",
                newName: "numericvector");

            migrationBuilder.RenameColumn(
                name: "MetricLength",
                table: "wordvector",
                newName: "metriclength");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "wordvector",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "todoitems",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "IsComplete",
                table: "todoitems",
                newName: "iscomplete");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "todoitems",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "hostPassword",
                table: "host",
                newName: "hostpassword");

            migrationBuilder.RenameColumn(
                name: "hostName",
                table: "host",
                newName: "hostname");

            migrationBuilder.RenameColumn(
                name: "hostId",
                table: "host",
                newName: "hostid");

            migrationBuilder.RenameIndex(
                name: "IX_host_hostId",
                table: "host",
                newName: "IX_host_hostid");

            migrationBuilder.CreateTable(
                name: "answer",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    roundid = table.Column<long>(type: "bigint", nullable: false),
                    playerid = table.Column<long>(type: "bigint", nullable: false),
                    answertarget = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_answer", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "player",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "quiz",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hostid = table.Column<long>(type: "bigint", nullable: false)
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
                    roundtargetid = table.Column<int>(type: "integer", nullable: false),
                    forbiddenwords = table.Column<string[]>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_round", x => x.id);
                    table.ForeignKey(
                        name: "FK_round_wordvector_roundtargetid",
                        column: x => x.roundtargetid,
                        principalTable: "wordvector",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_wordvector_wordornull",
                table: "wordvector",
                column: "wordornull",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_todoitems_id",
                table: "todoitems",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_answer_id",
                table: "answer",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_player_Id",
                table: "player",
                column: "Id",
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
                name: "IX_round_roundtargetid",
                table: "round",
                column: "roundtargetid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "answer");

            migrationBuilder.DropTable(
                name: "player");

            migrationBuilder.DropTable(
                name: "quiz");

            migrationBuilder.DropTable(
                name: "round");

            migrationBuilder.DropIndex(
                name: "IX_wordvector_wordornull",
                table: "wordvector");

            migrationBuilder.DropIndex(
                name: "IX_todoitems_id",
                table: "todoitems");

            migrationBuilder.RenameColumn(
                name: "wordornull",
                table: "wordvector",
                newName: "WordOrNull");

            migrationBuilder.RenameColumn(
                name: "numericvector",
                table: "wordvector",
                newName: "NumericVector");

            migrationBuilder.RenameColumn(
                name: "metriclength",
                table: "wordvector",
                newName: "MetricLength");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "wordvector",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "todoitems",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "iscomplete",
                table: "todoitems",
                newName: "IsComplete");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "todoitems",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "hostpassword",
                table: "host",
                newName: "hostPassword");

            migrationBuilder.RenameColumn(
                name: "hostname",
                table: "host",
                newName: "hostName");

            migrationBuilder.RenameColumn(
                name: "hostid",
                table: "host",
                newName: "hostId");

            migrationBuilder.RenameIndex(
                name: "IX_host_hostid",
                table: "host",
                newName: "IX_host_hostId");

            migrationBuilder.CreateIndex(
                name: "IX_host_hostName",
                table: "host",
                column: "hostName",
                unique: true);
        }
    }
}
