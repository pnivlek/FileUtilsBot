using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using SmileBot.Common.Attributes;
using SmileBot.Core.Services;
using SmileBot.Modules.Util.Services;

namespace SmileBot.Modules.Util
{
    public partial class Util : SmileHighLevelModule
    {
        [RequireOwner]
        [Group]
        public class ReactionCommands : SmileSubmodule
        {

            private readonly DbService _db;
            private readonly DiscordSocketClient _client;
            private readonly ReactionService _service;

            public ReactionCommands(DbService db, DiscordSocketClient client, ReactionService reactionService)
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
            public async Task ReactionTrackGetEmote([Leftover] string emote = null)
            {
                if (string.IsNullOrWhiteSpace(emote))
                    return;

            }
        }
    }
}
