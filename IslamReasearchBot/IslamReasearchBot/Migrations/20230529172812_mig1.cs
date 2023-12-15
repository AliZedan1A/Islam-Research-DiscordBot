using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IslamReasearchBot.Migrations
{
    /// <inheritdoc />
    public partial class mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServersData",
                columns: table => new
                {
                    ServerID = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoomsIds = table.Column<string>(type: "TEXT", nullable: false),
                    count = table.Column<int>(type: "INTEGER", nullable: false),
                    Admins = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServersData", x => x.ServerID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServersData");
        }
    }
}
