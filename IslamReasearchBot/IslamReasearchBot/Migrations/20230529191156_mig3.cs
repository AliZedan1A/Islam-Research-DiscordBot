using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IslamReasearchBot.Migrations
{
    /// <inheritdoc />
    public partial class mig3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServersData");

            migrationBuilder.CreateTable(
                name: "ServersAdmins",
                columns: table => new
                {
                    ServerId = table.Column<ulong>(type: "INTEGER", maxLength: 20, nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<ulong>(type: "INTEGER", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServersAdmins", x => x.ServerId);
                });

            migrationBuilder.CreateTable(
                name: "ServersCatigory",
                columns: table => new
                {
                    ServerId = table.Column<ulong>(type: "INTEGER", maxLength: 20, nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CatigoryId = table.Column<ulong>(type: "INTEGER", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServersCatigory", x => x.ServerId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServersAdmins");

            migrationBuilder.DropTable(
                name: "ServersCatigory");

            migrationBuilder.CreateTable(
                name: "ServersData",
                columns: table => new
                {
                    ServerID = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Admins = table.Column<string>(type: "TEXT", nullable: false),
                    RoomsIds = table.Column<string>(type: "TEXT", nullable: false),
                    count = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServersData", x => x.ServerID);
                });
        }
    }
}
