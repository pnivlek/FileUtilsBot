using System.Threading.Tasks;
using SmileBot.Core.Services.Database.Models;

namespace SmileBot.Core.Services.Database.Repositories
{
    public interface IQuoteRepository : IRepository<Quote>
    {
        Task<Quote> GetRandomQuote(ulong guildId);
        Task<Quote> GetRandomQuoteByKeyword(ulong guildId, string keyword);
    }
}