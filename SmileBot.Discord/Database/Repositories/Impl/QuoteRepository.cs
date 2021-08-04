using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmileBot.Discord.Database.Models;

namespace SmileBot.Discord.Database.Repositories.Impl
{
    public class QuoteRepository : Repository<Quote>, IQuoteRepository
    {
        public QuoteRepository(DbContext context) : base(context) { }
        public async Task<Quote> GetRandomQuote(ulong guildId)
        {
            Random random = new Random();
            return (await _set.AsQueryable().Where(q => q.GuildId == guildId).ToListAsync()).OrderBy(q => random.Next()).FirstOrDefault();
        }
        public async Task<Quote> GetRandomQuoteByKeyword(ulong guildId, string keyword)
        {
            Random random = new Random();
            return (await _set.AsQueryable().Where(q => q.GuildId == guildId && q.Keyword.Equals(keyword)).ToListAsync()).OrderBy(q => random.Next()).FirstOrDefault();
        }
    }
}