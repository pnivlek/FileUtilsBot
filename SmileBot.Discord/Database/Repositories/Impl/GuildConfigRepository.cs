using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SmileBot.Core.Database.Models;

namespace SmileBot.Core.Database.Repositories.Impl
{
    public class GuildConfigRepository : Repository<GuildConfig>, IGuildConfigRepository
    {
        public GuildConfigRepository(DbContext context) : base(context) { }

        public IEnumerable<GuildConfig> GetAllGuildConfigs(List<ulong> guildIds)
        {
            return AllConfigs().Where(gc => guildIds.Contains(gc.GuildId));
        }

        private IQueryable<GuildConfig> AllConfigs()
        {
            return _set.AsQueryable()
                .Include(gc => gc.ReactionTrackSettings);
        }

        public GuildConfig ById(ulong guildId, Func<DbSet<GuildConfig>, IQueryable<GuildConfig>> configs = null)
        {
            GuildConfig guild;
            if (configs == null)
            {
                guild = AllConfigs()
                    .FirstOrDefault(c => c.GuildId == guildId);
            }
            else
            {
                var set = configs(_set);
                guild = set.FirstOrDefault(c => c.GuildId == guildId);
            }

            if (guild == null)
            {
                _set.Add((guild = new GuildConfig
                {
                    GuildId = guildId
                }));
                _context.SaveChanges();
            }

            return guild;
        }
    }
}
