using System;
using System.Threading.Tasks;
using SmileBot.Discord.Database.Repositories;
using SmileBot.Discord.Database.Repositories.Impl;

namespace SmileBot.Discord.Database
{
    public class UnitOfWork : IUnitOfWork
    {
        public SmileContext _context { get; }

        private IUserRepository _userRepository;
        private IGuildConfigRepository _guildConfigRepository;
        private IQuoteRepository _quoteRepository;
        private IReactionEventRepository _reactionEventRepository;
        public IUserRepository Users => _userRepository ?? (_userRepository = new UserRepository(_context));
        public IGuildConfigRepository GuildConfigs => _guildConfigRepository ?? (_guildConfigRepository = new GuildConfigRepository(_context));
        public IQuoteRepository Quotes => _quoteRepository ?? (_quoteRepository = new QuoteRepository(_context));
        public IReactionEventRepository ReactionEvents => _reactionEventRepository ?? (_reactionEventRepository = new ReactionEventRepository(_context));

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
