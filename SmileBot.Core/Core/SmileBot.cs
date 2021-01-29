using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using SmileBot.Core.Database.Models;
using SmileBot.Core.Extensions;
using SmileBot.Core.Services;
using SmileBot.Core.Services.Impl;

namespace SmileBot
{
    public class SmileBot
    {
        private Logger _log;

        public BotCredentials Credentials { get; }
        public DiscordSocketClient Client { get; }
        public CommandService CommandService { get; }

        public ImmutableArray<GuildConfig> AllGuildConfigs { get; private set; }

        private readonly DbService _db;

        public static Color OkColor { get; set; }
        public static Color ErrorColor { get; set; }

        public IServiceProvider Services { get; private set; }

        public event Func<GuildConfig, Task> JoinedGuild = delegate { return Task.CompletedTask; };

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

            // TODO put this in a bot config db table.
            OkColor = new Color(Convert.ToUInt32("71CD40", 16));
            ErrorColor = new Color(Convert.ToUInt32("EE281F", 16));
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
            using (var uow = _db.GetDbContext())
            {
                var sw = Stopwatch.StartNew();

                AllGuildConfigs = uow.GuildConfigs.GetAllGuildConfigs(GetCurrentGuildIds()).ToImmutableArray();

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
                var commandHandler = Services.GetService<CommandHandler>();
                commandHandler.AddServices(s);
                LoadTypeReaders(typeof(SmileBot).Assembly);

                sw.Stop();
                _log.Info($"All services loaded in {sw.Elapsed.TotalSeconds:F2}s");
            }
        }

        private IEnumerable<object> LoadTypeReaders(Assembly assembly)
        {
            Type[] allTypes;
            try
            {
                allTypes = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                _log.Warn(ex.LoaderExceptions[0]);
                return Enumerable.Empty<object>();
            }
            var filteredTypes = allTypes
                .Where(x => x.IsSubclassOf(typeof(TypeReader))
                    && x.BaseType.GetGenericArguments().Length > 0
                    && !x.IsAbstract);

            var toReturn = new List<object>();
            foreach (var ft in filteredTypes)
            {
                var x = (TypeReader)Activator.CreateInstance(ft, Client, CommandService);
                var baseType = ft.BaseType;
                var typeArgs = baseType.GetGenericArguments();
                try
                {
                    CommandService.AddTypeReader(typeArgs[0], x);
                }
                catch (Exception ex)
                {
                    _log.Error(ex);
                    throw;
                }
                toReturn.Add(x);
            }

            return toReturn;
        }


        private async Task LoginAsync(string token)
        {
            // Make sure we are ready before we leave this method.
            var clientReady = new TaskCompletionSource<bool>();

            Task SetClientReady()
            {
                var _ = Task.Run(() =>
                {
                    clientReady.TrySetResult(true);
                });
                return Task.CompletedTask;
            }

            _log.Info("Shard {0} logging in...", Client.ShardId);
            await Client.LoginAsync(TokenType.Bot, token).ConfigureAwait(false);
            await Client.StartAsync().ConfigureAwait(false);
            Client.Ready += SetClientReady;
            await clientReady.Task.ConfigureAwait(false);
            Client.Ready -= SetClientReady;
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
            var _ = Task.Run(async () =>
            {
                GuildConfig gc;
                using (var uow = _db.GetDbContext())
                {
                    gc = uow.GuildConfigs.ById(arg.Id);
                }
                await JoinedGuild.Invoke(gc).ConfigureAwait(false);
            });
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

        public List<ulong> GetCurrentGuildIds()
        {
            return Client.Guilds.Select(g => g.Id).ToList<ulong>();
        }
    }
}
