using SmileBot.Core.Database.Models;

namespace SmileBot.Core.Database.Repositories
{
    public interface IReactionEventRepository : IRepository<ReactionEvent>
    {
        public void RemoveReaction(ulong guildEmoteId, ulong reactorUserId, ulong messageId);
        public void ClearReactions(ulong messageId);
    }
}
