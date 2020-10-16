using Microsoft.EntityFrameworkCore;
using SmileBot.Core.Database.Models;

namespace SmileBot.Core.Database.Repositories.Impl
{
    public class GuildConfigRepository : Repository<GuildConfig>, IGuildConfigRepository
    {
        public GuildConfigRepository(DbContext context) : base(context) { }

        public IEnumerable<GuildConfig> GetAllGuildConfigs()
    }
}