using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using SmileBot.Common.Attributes;
using SmileBot.Core.Services;
using SmileBot.Modules.ReactionTrack.Services;

namespace SmileBot.Modules.ReactionTrack
{
    [RequireOwner]
    public class ReactionCommands : SmileHighLevelModule
    {

        private readonly DbService _db;
        private readonly DiscordSocketClient _client;
        private readonly ReactionTrackService _service;

        public ReactionCommands(DbService db, DiscordSocketClient client, ReactionTrackService reactionService)
        {
            _db = db;
            _client = client;
            _service = reactionService;
        }

        [SmileCommand, Usage, Description, Aliases]
        public async Task ReactionTrackToggle()
        {
            bool enabled = _service.ReactionTrackToggle(ctx.Guild.Id);
            if (enabled)
            {
                await ReplyAsync("Custom reaction tracking enabled.");
            }
            else
            {
                await ReplyAsync("Custom reaction tracking disabled.");
            }
        }

        [SmileCommand, Usage, Description, Aliases]
        public async Task ReactionTrackGetEmote([Leftover] string emoteArg = null)
        {
            if (string.IsNullOrWhiteSpace(emoteArg))
                return;
            if (!Emote.TryParse(emoteArg, out Emote emote))
            {
                await ReplyAsync("Invalid emote.");
                return;
            }
            var stats = _service.ReactionTrackGetEmote(emote, ctx.Guild.Id);
            var embed = new EmbedBuilder()
                .WithTitle(":" + emote.Name + ":")
                .WithFields(
                    new EmbedFieldBuilder()
                    .WithName("Uses (Rank)")
                    .WithValue(stats)
                    .WithIsInline(false)
                )
                .WithThumbnailUrl(emote.Url);
            await ctx.Channel.SendMessageAsync(embed: embed.Build());
        }
    }
}
