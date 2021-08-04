using Microsoft.EntityFrameworkCore;
using SmileBot.Discord.Database.Models;

namespace SmileBot.Discord.Database.Repositories.Impl
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context) { }
    }
}