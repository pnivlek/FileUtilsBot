using Microsoft.EntityFrameworkCore;
using SmileBot.Core.Services.Database.Models;

namespace SmileBot.Core.Services.Database.Repositories.Impl
{
    public class GuildConfigRepository : Repository<GuildConfig>, IGuildConfigRepository
    {
        public GuildConfigRepository(DbContext context) : base(context) { }

        public IEnumerable<GuildConfig> GetAllGuildConfigs()
    }
}