using System;
using System.Threading.Tasks;
using SmileBot.Core.Services.Database.Repositories;
using SmileBot.Core.Services.Database.Repositories.Impl;

namespace SmileBot.Core.Services.Database
{
    public class UnitOfWork : IUnitOfWork
    {
        public SmileContext _context { get; }

        private IUserRepository _userRepository;
        private IGuildRepository _guildRepository;
        private IQuoteRepository _quoteRepository;
        public IUserRepository Users => _userRepository ?? (_userRepository = new UserRepository(_context));
        public IGuildRepository Guilds => _guildRepository ?? (_guildRepository = new GuildRepository(_context));
        public IQuoteRepository Quotes => _quoteRepository ?? (_quoteRepository = new QuoteRepository(_context));

        public UnitOfWork(SmileContext context)
        {
            _context = context;
        }
        public int SaveChanges() =>
            _context.SaveChanges();

        public Task<int> SaveChangesAsync() =>
            _context.SaveChangesAsync();

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}