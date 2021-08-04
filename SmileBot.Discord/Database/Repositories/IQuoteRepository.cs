using System.Threading.Tasks;
using SmileBot.Discord.Database.Models;

namespace SmileBot.Discord.Database.Repositories
{
    public interface IQuoteRepository : IRepository<Quote>
    {
        Task<Quote> GetRandomQuote(ulong guildId);
        Task<Quote> GetRandomQuoteByKeyword(ulong guildId, string keyword);
    }
}