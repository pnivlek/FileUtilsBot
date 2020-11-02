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
                .Where(x => x.EmojiId == guildEmoteId
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
    }
}
