using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmileBot.Core.Migrations
{
    public partial class Quotes : Migration
    {
        protected override void Up (MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable (
                name: "Quotes",
                columns : table => new
                {
                    Id = table.Column<int> (nullable: false)
                        .Annotation ("Sqlite:Autoincrement", true),
                        DateCreated = table.Column<DateTime> (nullable: true),
                        GuildId = table.Column<ulong> (nullable: false),
                        Keyword = table.Column<string> (nullable: true),
                        AuthorId = table.Column<ulong> (nullable: false),
                        Author = table.Column<string> (nullable: true),
                        Text = table.Column<string> (nullable: true)
                },
                constraints : table =>
                {
                    table.PrimaryKey ("PK_Quotes", x => x.Id);
                });
        }

        protected override void Down (MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable (
                name: "Quotes");
        }
    }
}