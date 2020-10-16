using Microsoft.EntityFrameworkCore;
using SmileBot.Core.Database.Models;

namespace SmileBot.Core.Database.Repositories.Impl
{
    public class GuildRepository : Repository<GuildConfig>, IGuildConfigRepository
    {
        public GuildRepository(DbContext context) : base(context) { }
    }
}