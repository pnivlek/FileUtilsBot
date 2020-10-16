using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Net;
using System.Reflection;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using SmileBot.Core.Extensions;
using SmileBot.Core.Services;
using SmileBot.Core.Services.Database.Models;
using SmileBot.Core.Services.Impl;

namespace SmileBot
{
    public class SmileBot
    {
        private Logger _log;

        public BotCredentials Credentials { get; }
        public DiscordSocketClient Client { get; }
        public CommandService CommandService { get; }

        private readonly DbService _db;

        public IServiceProvider Services { get; private set; }

        public SmileBot()
        {
            LogSetup.SetupLogger(1);
            _log = LogManager.GetCurrentClassLogger();

            Credentials = new BotCredentials();
            Client = new DiscordSocketClient();
            CommandService = new CommandService(new CommandServiceConfig()
            {
                CaseSensitiveCommands = false,
                    DefaultRunMode = RunMode.Async,
            });

            _db = new DbService(Credentials);
            _db.Setup();

            Client.Log += Client_Log;
        }

        public async Task RunAsync()
        {
            var sw = Stopwatch.StartNew();

            await LoginAsync(Credentials.Token).ConfigureAwait(false);

            _log.Info($"Shard {Client.ShardId} loading services...");
            try
            {
                AddServices();
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                throw;
            }

            sw.Stop();
            _log.Info($"Shard {Client.ShardId} connected in {sw.Elapsed.TotalSeconds:F2}s");

            var commandHandler = Services.GetService<CommandHandler>();
            var CommandService = Services.GetService<CommandService>();

            await commandHandler.StartHandling().ConfigureAwait(false);

            var _ = await CommandService.AddModulesAsync(this.GetType().GetTypeInfo().Assembly, Services)
                .ConfigureAwait(false);

            _log.Info($"Shard {Client.ShardId} ready.");
        }

        private void AddServices()
        {
            // TODO: uow pattern
            var sw = Stopwatch.StartNew();

            var s = new ServiceCollection()
                .AddSingleton<IBotCredentials>(Credentials)
                .AddSingleton<InteractiveService>()
                .AddSingleton<Random>()
                .AddSingleton(_db)
                .AddSingleton(Client)
                .AddSingleton(CommandService)
                .AddSingleton(this)
                .AddMemoryCache();

            s.LoadFrom(Assembly.GetAssembly(typeof(CommandHandler)));

            Services = s.BuildServiceProvider();
            // TODO: Decide what to do with this.
            var commandHandler = Services.GetService<CommandHandler>();
            commandHandler.AddServices(s);

            sw.Stop();
            _log.Info($"All services loaded in {sw.Elapsed.TotalSeconds:F2}s");
        }

        private async Task LoginAsync(string token)
        {
            _log.Info("Shard {0} logging in...", Client.ShardId);
            await Client.LoginAsync(TokenType.Bot, token).ConfigureAwait(false);
            await Client.StartAsync().ConfigureAwait(false);
            Client.JoinedGuild += Client_JoinedGuild;
            Client.LeftGuild += Client_LeftGuild;
            _log.Info("Shard {0} logged in.", Client.ShardId);
        }

        private Task Client_Log(LogMessage arg)
        {
            _log.Warn(arg.Source + " | " + arg.Message);
            if (arg.Exception != null)
                _log.Warn(arg.Exception);

            return Task.CompletedTask;
        }

        private Task Client_JoinedGuild(SocketGuild arg)
        {
            _log.Info("Joined server: {0} [{1}]", arg?.Name, arg?.Id);
            return Task.CompletedTask;
        }

        private Task Client_LeftGuild(SocketGuild arg)
        {
            _log.Info("Left server: {0} [{1}]", arg?.Name, arg?.Id);
            return Task.CompletedTask;
        }

        public async Task RunAndBlockAsync()
        {
            await RunAsync().ConfigureAwait(false);
            await Task.Delay(-1).ConfigureAwait(false);
        }
    }
}