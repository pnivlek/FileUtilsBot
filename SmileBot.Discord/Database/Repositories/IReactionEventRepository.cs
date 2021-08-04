using Discord;
using SmileBot.Discord.Database.Models;

namespace SmileBot.Discord.Database.Repositories
{
    public interface IReactionEventRepository : IRepository<ReactionEvent>
    {
        public void RemoveReaction(ulong guildEmoteId, ulong reactorUserId, ulong messageId);
        public void ClearReactions(ulong messageId);
        public SingleEmoteStats GetSingleEmoteStats(ulong guildEmoteId, ulong guildId);
    }

    public struct SingleEmoteStats
    {
        public ulong EmoteId { get; set; }
        public int RankWeek { get; set; }
        public int LastWeekUsage { get; set; }
        public int RankMonth { get; set; }
        public int LastMonthUsage { get; set; }
        public int RankTwoMonths { get; set; }
        public int LastTwoMonthsUsage { get; set; }

        public override string ToString()
        {
            return string.Format("Week: {0} ({1})\nMonth: {2} ({3})\nTwo Months: {4} ({5})",
                                 LastWeekUsage, RankWeek,
                                 LastMonthUsage, RankMonth,
                                 LastTwoMonthsUsage, RankTwoMonths);
        }
    }
}
