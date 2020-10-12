using SmileBot.Core.Services.Database.Repositories;
using SmileBot.Core.Services.Database.Repositories.Impl;

namespace SmileBot.Core.Services.Database
{
    public class UnitOfWork : IUnitOfWork
    {
        public SmileContext _context { get; }

        private IUserRepository _userRepository;
        public IUserRepository UserRepository => _userRepository ?? (_userRepository = new UserRepository(_context));

        public UnitOfWork(SmileContext context)
        {
            _context = context;
        }
    }
}