using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IslamReasearchBot.Migrations
{
    /// <inheritdoc />
    public partial class mig4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ServersConfig",
                table: "ServersConfig");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServersCatigory",
                table: "ServersCatigory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServersAdmins",
                table: "ServersAdmins");

            migrationBuilder.AlterColumn<ulong>(
                name: "ServerID",
                table: "ServersConfig",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(ulong),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "ServersConfig",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<ulong>(
                name: "ServerId",
                table: "ServersCatigory",
                type: "INTEGER",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(ulong),
                oldType: "INTEGER",
                oldMaxLength: 20)
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "ServersCatigory",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<ulong>(
                name: "ServerId",
                table: "ServersAdmins",
                type: "INTEGER",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(ulong),
                oldType: "INTEGER",
                oldMaxLength: 20)
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "ServersAdmins",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServersConfig",
                table: "ServersConfig",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServersCatigory",
                table: "ServersCatigory",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServersAdmins",
                table: "ServersAdmins",
                column: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ServersConfig",
                table: "ServersConfig");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServersCatigory",
                table: "ServersCatigory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServersAdmins",
                table: "ServersAdmins");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "ServersConfig");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "ServersCatigory");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "ServersAdmins");

            migrationBuilder.AlterColumn<ulong>(
                name: "ServerID",
                table: "ServersConfig",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(ulong),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<ulong>(
                name: "ServerId",
                table: "ServersCatigory",
                type: "INTEGER",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(ulong),
                oldType: "INTEGER",
                oldMaxLength: 20)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<ulong>(
                name: "ServerId",
                table: "ServersAdmins",
                type: "INTEGER",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(ulong),
                oldType: "INTEGER",
                oldMaxLength: 20)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServersConfig",
                table: "ServersConfig",
                column: "ServerID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServersCatigory",
                table: "ServersCatigory",
                column: "ServerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServersAdmins",
                table: "ServersAdmins",
                column: "ServerId");
        }
    }
}
