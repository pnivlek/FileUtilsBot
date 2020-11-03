using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SmileBot.Core.Database.Models;

namespace SmileBot.Core.Database.Repositories.Impl
{
    public class ReactionEventRepository : Repository<ReactionEvent>, IReactionEventRepository
    {
        public ReactionEventRepository(DbContext context) : base(context) { }

        public void RemoveReaction(ulong guildEmoteId, ulong reactorUserId, ulong messageId)
        {
            var re = _set
                .AsQueryable()
                .Where(x => x.EmoteId == guildEmoteId
                       && x.ReactorUserId == reactorUserId
                       && x.MessageId == messageId)
                .FirstOrDefault();
            _set.Remove(re);
        }

        public void ClearReactions(ulong messageId)
        {
            var re = _set
                .AsQueryable()
                .Where(x => x.MessageId == messageId);
            _set.RemoveRange(re);
        }

        public SingleEmoteStats GetSingleEmoteStats(ulong guildEmoteId, ulong guildId)
        {
            var lastWeekEmote = GetSingleEmoteStatsForOffset(guildEmoteId, guildId, -7);
            var lastMonthEmote = GetSingleEmoteStatsForOffset(guildEmoteId, guildId, -30);
            var lastTwoMonthsEmote = GetSingleEmoteStatsForOffset(guildEmoteId, guildId, -60);
            var stats = new SingleEmoteStats
            {
                EmoteId = guildEmoteId,
                RankWeek = lastWeekEmote.Rank,
                LastWeekUsage = lastWeekEmote.Count,
                RankMonth = lastMonthEmote.Rank,
                LastMonthUsage = lastMonthEmote.Count,
                RankTwoMonths = lastTwoMonthsEmote.Rank,
                LastTwoMonthsUsage = lastTwoMonthsEmote.Count,
            };
            return stats;
        }

        private (int Count, int Rank) GetSingleEmoteStatsForOffset(ulong guildEmoteId, ulong guildId, double days)
        {
            var emoteTable = _context.Set<ReactionEvent>()
                .AsQueryable()
                .Where(e => e.GuildId == guildId && e.DateCreated > DateTime.Now.AddDays(days))
                .GroupBy(e => e.EmoteId)
                .Select(g => new { EmojiId = g.Key, Count = g.Count() })
                .OrderByDescending(e => e.Count)
                .ToList() // Needed for EF Core. There shouldn't be more than 100-250 members of this list.
                .Select((e, i) => new
                {
                    e.EmojiId,
                    e.Count,
                    Rank = i + 1
                })
                .Where(e => e.EmojiId == guildEmoteId)
                .FirstOrDefault();
            if (emoteTable == null)
            {
                return (0, 0);
            }
            return (emoteTable.Count, emoteTable.Rank);
        }
    }
}
