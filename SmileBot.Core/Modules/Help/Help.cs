using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using SmileBot.Common.Attributes;
using SmileBot.Core.Extensions;
using SmileBot.Core.Services;

namespace SmileBot.Modules.Help
{
    public partial class Help : SmileHighLevelModule
    {
        private readonly DiscordSocketClient _client;
        private readonly IBotCredentials _creds;
        private readonly CommandService _cmds;

        public Help(DiscordSocketClient client, IBotCredentials creds, CommandService cmds)
        {
            _client = client;
            _creds = creds;
            _cmds = cmds;
        }

        [SmileCommand, Usage, Description, Aliases]
        public async Task Modules()
        {
            var embed = new EmbedBuilder().WithFields(new EmbedFieldBuilder().WithName("List of modules").WithValue(string.Join("\n", _cmds.Modules.GroupBy(m => m.GetTopLevelModule()).Select(m => "- " + m.Key.Name).OrderBy(m => m))));
            await ctx.Channel.EmbedAsync(embed).ConfigureAwait(false);
        }
    }
}