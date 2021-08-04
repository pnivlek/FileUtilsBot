using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using SmileBot.Discord.Services;

namespace SmileBot.Common.TypeReaders
{
    public class BotCommandTypeReader : SmileTypeReader<CommandInfo>
    {
        public BotCommandTypeReader(DiscordSocketClient client, CommandService cmds) : base(client, cmds)
        {
        }

        public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
        {
            var _cmds = services.GetService<CommandService>();
            var _cmdHandler = services.GetService<CommandHandler>();
            input = input.ToUpperInvariant();
            var prefix = "."; // TODO add prefix customization
            if (!input.StartsWith(prefix.ToUpperInvariant(), StringComparison.InvariantCulture))
                return Task.FromResult(TypeReaderResult.FromError(CommandError.ParseFailed, "No such command found."));

            input = input.Substring(prefix.Length);

            var cmd = _cmds.Commands.FirstOrDefault(c =>
                c.Aliases.Select(a => a.ToUpperInvariant()).Contains(input));
            if (cmd == null)
                return Task.FromResult(TypeReaderResult.FromError(CommandError.ParseFailed, "No such command found."));

            return Task.FromResult(TypeReaderResult.FromSuccess(cmd));
        }
    }
}
