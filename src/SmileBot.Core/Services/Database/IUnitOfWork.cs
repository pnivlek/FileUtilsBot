using System;
using System.Threading.Tasks;
using SmileBot.Core.Services.Database.Repositories;
namespace SmileBot.Core.Services.Database
{
    public interface IUnitOfWork : IDisposable
    {
        SmileContext _context { get; }
        IUserRepository Users { get; }
        IGuildRepository Guilds { get; }
        IQuoteRepository Quotes { get; }
        int SaveChanges ();
        Task<int> SaveChangesAsync ();
    }
}