using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToX.Migrations
{
    /// <inheritdoc />
    public partial class overwriteSchema_host : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_user",
                table: "user");

            migrationBuilder.RenameTable(
                name: "user",
                newName: "host");

            migrationBuilder.RenameColumn(
                name: "userPassword",
                table: "host",
                newName: "hostPassword");

            migrationBuilder.RenameColumn(
                name: "userName",
                table: "host",
                newName: "hostName");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "host",
                newName: "hostId");

            migrationBuilder.RenameIndex(
                name: "IX_user_userName",
                table: "host",
                newName: "IX_host_hostName");

            migrationBuilder.RenameIndex(
                name: "IX_user_userId",
                table: "host",
                newName: "IX_host_hostId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_host",
                table: "host",
                column: "hostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_host",
                table: "host");

            migrationBuilder.RenameTable(
                name: "host",
                newName: "user");

            migrationBuilder.RenameColumn(
                name: "hostPassword",
                table: "user",
                newName: "userPassword");

            migrationBuilder.RenameColumn(
                name: "hostName",
                table: "user",
                newName: "userName");

            migrationBuilder.RenameColumn(
                name: "hostId",
                table: "user",
                newName: "userId");

            migrationBuilder.RenameIndex(
                name: "IX_host_hostName",
                table: "user",
                newName: "IX_user_userName");

            migrationBuilder.RenameIndex(
                name: "IX_host_hostId",
                table: "user",
                newName: "IX_user_userId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user",
                table: "user",
                column: "userId");
        }
    }
}
