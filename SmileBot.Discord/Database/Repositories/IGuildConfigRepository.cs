using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SmileBot.Discord.Database.Models;

namespace SmileBot.Discord.Database.Repositories
{
    public interface IGuildConfigRepository : IRepository<GuildConfig>
    {
        IEnumerable<GuildConfig> GetAllGuildConfigs(List<ulong> guildIds);
        GuildConfig ById(ulong guildId, Func<DbSet<GuildConfig>, IQueryable<GuildConfig>> config = null);
    }
}
