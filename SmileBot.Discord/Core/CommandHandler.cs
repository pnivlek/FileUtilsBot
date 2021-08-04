using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace SmileBot.Discord.Services
{
    public class CommandHandler : ISmileService
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commandService;
        private readonly Logger _log;
        private readonly IBotCredentials _creds;
        private readonly SmileBot _bot;
        private IServiceProvider _services;

        public string DefaultPrefix { get; private set; }
        private ConcurrentDictionary<ulong, string> _prefixes { get; } = new ConcurrentDictionary<ulong, string>();

        public CommandHandler(DiscordSocketClient client, CommandService commandService, IBotCredentials credentials, SmileBot bot, IServiceProvider services)
        {
            _client = client;
            _commandService = commandService;
            _creds = credentials;
            _bot = bot;
            _services = services;

            _log = LogManager.GetCurrentClassLogger();
            DefaultPrefix = ".";
        }

        public Task StartHandling()
        {
            _client.MessageReceived += (msg) => { var _ = Task.Run(() => MessageReceivedHandler(msg)); return Task.CompletedTask; };
            return Task.CompletedTask;
        }

        private async Task MessageReceivedHandler(SocketMessage msg)
        {
            try
            {
                if (msg.Author.IsBot)
                    return;

                if (!(msg is SocketUserMessage usrMsg))
                    return;

                var channel = msg.Channel as ISocketMessageChannel;
                var guild = (msg.Channel as SocketTextChannel)?.Guild;

                await TryRunCommand(guild, channel, usrMsg).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _log.Warn("Error in CommandHandler");
                _log.Warn(ex);
                if (ex.InnerException != null)
                {
                    _log.Warn("Inner Exception of the error in CommandHandler");
                    _log.Warn(ex.InnerException);
                }
            }
        }

        public async Task TryRunCommand(SocketGuild guild, ISocketMessageChannel channel, IUserMessage usrMsg)
        {
            // TODO: limit who can run commands?
            var messageContent = usrMsg.Content;
            var prefix = DefaultPrefix;
            var isPrefixCommand = messageContent.StartsWith(".prefix", StringComparison.InvariantCultureIgnoreCase);

            if (messageContent.StartsWith(prefix, StringComparison.InvariantCulture) || isPrefixCommand)
            {
                var (Success, Error, Info) = await ExecuteCommandAsync(new CommandContext(_client, usrMsg), messageContent, isPrefixCommand ? 1 : prefix.Length, _services).ConfigureAwait(false);
            }
        }

        public void AddServices(IServiceCollection s) { }

        private Task<(bool Success, string Error, CommandInfo Info)> ExecuteCommandAsync(CommandContext context, string input, int argPos, IServiceProvider serviceProvider) => ExecuteCommand(context, input.Substring(argPos), serviceProvider);

        private async Task<(bool Success, string Error, CommandInfo Info)> ExecuteCommand(CommandContext context, string input, IServiceProvider services)
        {
            var execResult = (ExecuteResult)await _commandService.ExecuteAsync(context, 1, services: services);
            // note: unknown commands are not logged since the above statement moves out of the method.
            if (execResult.Exception != null && (!(execResult.Exception is HttpException he) || he.DiscordCode != 50013))
            {
                lock (errorLogLock)
                {
                    var now = DateTime.Now;
                    File.AppendAllText($"./command_errors_{now:yyyy-MM-dd}.txt",
                        $"[{now:HH:mm-yyyy-MM-dd}]" + Environment.NewLine +
                        execResult.Exception.ToString() + Environment.NewLine +
                        "------" + Environment.NewLine);
                    _log.Warn(execResult.Exception);
                }
            }

            return (true, null, null); // TODO: info.
        }

        private readonly object errorLogLock = new object();
    }
}