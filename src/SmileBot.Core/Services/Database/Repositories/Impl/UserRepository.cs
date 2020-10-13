using Microsoft.EntityFrameworkCore;
using SmileBot.Core.Services.Database.Models;

namespace SmileBot.Core.Services.Database.Repositories.Impl
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context) { }
    }
}