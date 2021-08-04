using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using NLog;
using SmileBot.Discord.Database.Models;
using SmileBot.Discord.Database.Repositories;
using SmileBot.Discord.Services;

namespace SmileBot.Modules.ReactionTrack.Services
{
    public class ReactionTrackService : ISmileService
    {
        private readonly Logger _log;
        private readonly CommandHandler _cmd;
        private readonly SmileBot _bot;
        private readonly DiscordSocketClient _client;
        private readonly DbService _db;
        private readonly ConcurrentDictionary<ulong, ReactionTrackSettings> _reactionSettingsCache;

        public ReactionTrackService(CommandHandler cmd, SmileBot bot, DiscordSocketClient client, DbService db)
        {
            _cmd = cmd;
            _bot = bot;
            _log = LogManager.GetCurrentClassLogger();
            _db = db;
            _client = client;

            _reactionSettingsCache = new ConcurrentDictionary<ulong, ReactionTrackSettings>(_bot.AllGuildConfigs.ToDictionary(g => g.GuildId, g => g.ReactionTrackSettings));

            _bot.JoinedGuild += _bot_JoinedGuild;
            _client.LeftGuild += _client_LeftGuild;
            _client.ReactionAdded += _client_ReactionAdded;
            _client.ReactionRemoved += _client_ReactionRemoved;
            _client.ReactionsCleared += _client_ReactionsCleared;
        }

        private Task _bot_JoinedGuild(GuildConfig gc)
        {
            using (var uow = _db.GetDbContext())
            {
                var gcWithData = uow.GuildConfigs.ById(gc.GuildId,
                                                 x => x.Include(x => x.ReactionTrackSettings));
                Initialize(gcWithData);
            }
            return Task.CompletedTask;
        }

        private Task _client_LeftGuild(SocketGuild g)
        {
            _reactionSettingsCache.TryRemove(g.Id, out _);
            return Task.CompletedTask;
        }

        private Task Initialize(GuildConfig gc)
        {
            if (gc.ReactionTrackSettings == null)
            {
                _reactionSettingsCache.TryAdd(gc.GuildId, new ReactionTrackSettings { TrackEnabled = false, IgnoredChannels = new List<ulong>() });
            }
            else
            {
                _reactionSettingsCache.TryAdd(gc.GuildId, gc.ReactionTrackSettings);
            }
            return Task.CompletedTask;
        }

        private Task _client_ReactionAdded(Cacheable<IUserMessage, ulong> cache, IMessageChannel channel, SocketReaction reaction) => _client_ReactionAddedAsync(cache, channel, reaction);

        private async Task _client_ReactionAddedAsync(Cacheable<IUserMessage, ulong> cache, IMessageChannel channel, SocketReaction reaction)
        {
            if (!ValidateReaction(channel))
                return;
            var guildId = ((SocketGuildChannel)channel).Guild.Id;
            var emote = reaction.Emote as Emote;

            using (var uow = _db.GetDbContext())
            {
                uow.ReactionEvents.Add(new ReactionEvent
                {
                    EmoteId = emote.Id,
                    EmoteName = emote.Name,
                    MessageId = cache.Id,
                    ChannelId = channel.Id,
                    GuildId = guildId,
                    FromOutsideGuild = !_client.GetGuild(guildId).Emotes.Any(x => x.Id == (reaction.Emote as Emote).Id),
                    ReactorUserId = reaction.UserId,
                });
                await uow.SaveChangesAsync();
            }
        }

        private Task _client_ReactionRemoved(Cacheable<IUserMessage, ulong> cache, IMessageChannel channel, SocketReaction reaction) => _client_ReactionRemovedAsync(cache, channel, reaction);

        private async Task _client_ReactionRemovedAsync(Cacheable<IUserMessage, ulong> cache, IMessageChannel channel, SocketReaction reaction)
        {
            if (!ValidateReaction(channel))
                return;

            using (var uow = _db.GetDbContext())
            {
                uow.ReactionEvents.RemoveReaction((reaction.Emote as Emote).Id, reaction.UserId, cache.Id);
                await uow.SaveChangesAsync();
            }
        }

        private Task _client_ReactionsCleared(Cacheable<IUserMessage, ulong> cache, ISocketMessageChannel channel) => _client_ReactionsClearedAsync(cache);

        private async Task _client_ReactionsClearedAsync(Cacheable<IUserMessage, ulong> cache)
        {
            using (var uow = _db.GetDbContext())
            {
                uow.ReactionEvents.ClearReactions(cache.Id);
                await uow.SaveChangesAsync();
            }
        }

        public bool ReactionTrackToggle(ulong guildId)
        {
            bool enabled;
            using (var uow = _db.GetDbContext())
            {
                var rt = uow.GuildConfigs.ById(guildId, set => set.Include(x => x.ReactionTrackSettings)).ReactionTrackSettings;
                enabled = rt.TrackEnabled = !rt.TrackEnabled;
                UpdateCache(guildId, rt);
                uow.SaveChanges();
            }
            return enabled;
        }

        public SingleEmoteStats ReactionTrackGetEmote(Emote emote, ulong guildId)
        {
            using (var uow = _db.GetDbContext())
            {
                var stats = uow.ReactionEvents.GetSingleEmoteStats(emote.Id, guildId);
                return stats;
            }
        }

        private bool ValidateReaction(IMessageChannel channel)
        {
            if (!_reactionSettingsCache.TryGetValue(((SocketGuildChannel)channel).Guild.Id, out ReactionTrackSettings settings) || !settings.TrackEnabled)
                return false;
            if (settings.IgnoredChannels.Contains(channel.Id))
                return false;
            return true;
        }

        private void UpdateCache(ulong guildId, ReactionTrackSettings setting)
        {
            _reactionSettingsCache.AddOrUpdate(guildId, (key) => setting, (key, old) => setting);
        }
    }
}
