using Microsoft.EntityFrameworkCore.Migrations;

namespace SmileBot.Discord.Migrations
{
    public partial class ReactionTrackRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmojiId",
                table: "ReactionEvents",
                newName: "EmoteId");

            migrationBuilder.RenameColumn(
                name: "EmojiName",
                table: "ReactionEvents",
                newName: "EmoteName");

            migrationBuilder.AddColumn<bool>(
                name: "FromOutsideGuild",
                table: "ReactionEvents",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromOutsideGuild",
                table: "ReactionEvents");

            migrationBuilder.RenameColumn(
                name: "EmoteId",
                table: "ReactionEvents",
                newName: "EmojiId");

            migrationBuilder.RenameColumn(
                name: "EmoteName",
                table: "ReactionEvents",
                newName: "EmojiName");

        }
    }
}
