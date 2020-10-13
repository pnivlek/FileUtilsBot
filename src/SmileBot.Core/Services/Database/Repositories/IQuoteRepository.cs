using System.Threading.Tasks;
using SmileBot.Core.Services.Database.Models;

namespace SmileBot.Core.Services.Database.Repositories
{
    public interface IQuoteRepository : IRepository<Quote>
    {
        Task<Quote> GetRandomFromGuild (ulong guildId);
        Task<Quote> GetRandomFromGuildByKeyword (ulong guildId, string keyword);
    }
}