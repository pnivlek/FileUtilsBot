using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SmileBot.Core.Database.Models;

namespace SmileBot.Core.Database.Repositories
{
    public interface IGuildConfigRepository : IRepository<GuildConfig>
    {
        IEnumerable<GuildConfig> GetAllGuildConfigs(List<ulong> guildIds);
        GuildConfig ById(ulong guildId, Func<DbSet<GuildConfig>, IQueryable<GuildConfig>> config = null);
    }
}
