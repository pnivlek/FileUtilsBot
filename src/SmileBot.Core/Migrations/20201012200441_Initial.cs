using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmileBot.Core.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up (MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable (
                name: "BotConfig",
                columns : table => new
                {
                    Id = table.Column<int> (nullable: false)
                        .Annotation ("Sqlite:Autoincrement", true),
                        DateCreated = table.Column<DateTime> (nullable: true)
                },
                constraints : table =>
                {
                    table.PrimaryKey ("PK_BotConfig", x => x.Id);
                });

            migrationBuilder.CreateTable (
                name: "Guilds",
                columns : table => new
                {
                    Id = table.Column<int> (nullable: false)
                        .Annotation ("Sqlite:Autoincrement", true),
                        DateCreated = table.Column<DateTime> (nullable: true),
                        GuildId = table.Column<ulong> (nullable: false),
                        Prefix = table.Column<string> (nullable: true),
                        DeleteMessageOnCommand = table.Column<bool> (nullable: false),
                        DeleteMessageOnCommandTimer = table.Column<int> (nullable: false)
                },
                constraints : table =>
                {
                    table.PrimaryKey ("PK_Guilds", x => x.Id);
                });

            migrationBuilder.CreateTable (
                name: "Users",
                columns : table => new
                {
                    Id = table.Column<int> (nullable: false)
                        .Annotation ("Sqlite:Autoincrement", true),
                        DateCreated = table.Column<DateTime> (nullable: true),
                        UserId = table.Column<ulong> (nullable: false),
                        Username = table.Column<string> (nullable: true),
                        Discriminator = table.Column<string> (nullable: true),
                        AvatarId = table.Column<string> (nullable: true)
                },
                constraints : table =>
                {
                    table.PrimaryKey ("PK_Users", x => x.Id);
                });
        }

        protected override void Down (MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable (
                name: "BotConfig");

            migrationBuilder.DropTable (
                name: "Guilds");

            migrationBuilder.DropTable (
                name: "Users");
        }
    }
}