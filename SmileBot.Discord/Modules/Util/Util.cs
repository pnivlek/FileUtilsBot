using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using SmileBot.Common.Attributes;
using SmileBot.Discord.Extensions;
using SmileBot.Discord.Services;

namespace SmileBot.Modules.Util
{
    public partial class Util : SmileHighLevelModule
    {
        private readonly DiscordSocketClient _client;
        private readonly IBotCredentials _creds;
        private readonly SmileBot _bot;
        private readonly DbService _db;

        public Util(SmileBot smile, DiscordSocketClient client,
            IBotCredentials creds, DbService db)
        {
            _client = client;
            _creds = creds;
            _bot = smile;
            _db = db;
        }

    }
}