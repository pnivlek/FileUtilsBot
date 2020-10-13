using Microsoft.EntityFrameworkCore;
using SmileBot.Core.Services.Database.Models;

namespace SmileBot.Core.Services.Database.Repositories.Impl
{
    public class GuildRepository : Repository<Guild>, IGuildRepository
    {
        public GuildRepository(DbContext context) : base(context) { }
    }
}