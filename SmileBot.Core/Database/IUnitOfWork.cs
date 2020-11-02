using System;
using System.Threading.Tasks;
using SmileBot.Core.Database.Repositories;
namespace SmileBot.Core.Database
{
    public interface IUnitOfWork : IDisposable
    {
        SmileContext _context { get; }
        IUserRepository Users { get; }
        IGuildConfigRepository GuildConfigs { get; }
        IQuoteRepository Quotes { get; }
        IReactionEventRepository ReactionEvents { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
