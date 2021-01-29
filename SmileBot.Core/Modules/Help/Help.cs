using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using SmileBot.Common.Attributes;
using SmileBot.Core.Extensions;
using SmileBot.Core.Services;
using SmileBot.Modules.Help.Services;

namespace SmileBot.Modules.Help
{
    public partial class Help : SmileHighLevelModule<HelpService>
    {
        private readonly DiscordSocketClient _client;
        private readonly IBotCredentials _creds;
        private readonly CommandService _cmds;

        public Help(DiscordSocketClient client, IBotCredentials creds, CommandService cmds, HelpService service)
        {
            _client = client;
            _creds = creds;
            _cmds = cmds;
        }

        [SmileCommand, Usage, Description, Aliases]
        public async Task Modules()
        {
            var embed = new EmbedBuilder().WithFields(
                new EmbedFieldBuilder().WithName("List of modules").WithValue(
                    string.Join("\n", _cmds.Modules.Select(m => m.Name).OrderBy(m => m))));
            await ctx.Channel.EmbedAsync(embed).ConfigureAwait(false);
        }

        // TODO: Check user permissions to filter commands that cannot be run.
        [SmileCommand, Usage, Description, Aliases]
        public async Task Commands(string module)
        {
            var cmdsInModule = _cmds.Commands.Where(c => c.Module.GetTopLevelModule().Name.Equals(module));
            if (cmdsInModule.Count() == 0)
            {
                await ReplyAsync("Unknown module.");
                return;
            }
            var embed = new EmbedBuilder()
                .WithDescription("- " + string.Join("\n- ",
                                                    cmdsInModule.Select(c => string.Join(" / ", c.Aliases.Select(a => Format.Code(a))))))
                .WithFooter($"Type {CmdHandler.DefaultPrefix}h CommandName to see the help for that specified command.");
            await ctx.Channel.EmbedAsync(embed).ConfigureAwait(false);
        }

        [SmileCommand, Usage, Description, Aliases]
        [Priority(0)]
        public async Task H([Leftover] string fail)
        {
            if (string.IsNullOrWhiteSpace(fail))
            {
                await ctx.Message.Author.EmbedAsync(HelpService.GetBotHelp());
                return;
            }
            CommandInfo cmd = _cmds.Commands.First(m => m.Aliases.Contains(fail));
            if (cmd is null)
            {
                await ReplyAsync(ctx.Message.Author.Mention + $" command {fail} was not found.").ConfigureAwait(false);
                return;
            }
            await H(cmd);
        }

        [SmileCommand, Usage, Description, Aliases]
        [Priority(1)]
        public async Task H([Leftover] CommandInfo cmd = null)
        {
            var embed = _service.GetCommandHelp(cmd, ctx.Guild);
            await ctx.Channel.EmbedAsync(embed).ConfigureAwait(false);
        }
    }
}
