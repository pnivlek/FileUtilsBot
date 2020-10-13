using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmileBot.Core.Services.Database.Models;

namespace SmileBot.Core.Services.Database.Repositories.Impl
{
    public class QuoteRepository : Repository<Quote>, IQuoteRepository
    {
        public QuoteRepository(DbContext context) : base(context) { }
        public async Task<Quote> GetRandomFromGuild(ulong guildId)
        {
            Random random = new Random();
            return (await _set.AsQueryable().Where(q => q.GuildId == guildId).ToListAsync()).OrderBy(q => random.Next()).FirstOrDefault();
        }
        public async Task<Quote> GetRandomFromGuildByKeyword(ulong guildId, string keyword)
        {
            Random random = new Random();
            return (await _set.AsQueryable().Where(q => q.GuildId == guildId && q.Keyword.Equals(keyword)).ToListAsync()).OrderBy(q => random.Next()).FirstOrDefault();
        }
    }
}