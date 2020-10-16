using Microsoft.EntityFrameworkCore;
using SmileBot.Core.Services.Database.Models;

namespace SmileBot.Core.Services.Database.Repositories.Impl
{
    public class GuildRepository : Repository<GuildConfig>, IGuildConfigRepository
    {
        public GuildRepository(DbContext context) : base(context) { }
    }
}