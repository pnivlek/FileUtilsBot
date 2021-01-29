using Discord.Commands;
using Discord.WebSocket;

namespace SmileBot.Common.TypeReaders
{
    public abstract class SmileTypeReader<T> : TypeReader
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _cmds;

        private SmileTypeReader() { }
        protected SmileTypeReader(DiscordSocketClient client, CommandService cmds)
        {
            _client = client;
            _cmds = cmds;
        }
    }
}
