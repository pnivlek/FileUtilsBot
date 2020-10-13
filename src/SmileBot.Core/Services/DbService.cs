using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SmileBot.Core.Services.Database;

namespace SmileBot.Core.Services
{
    public class DbService
    {
        private readonly DbContextOptions<SmileContext> options;
        private readonly DbContextOptions<SmileContext> migrateOptions;

        public DbService(IBotCredentials creds)
        {
            var builder = new SqliteConnectionStringBuilder(creds.Db.ConnectionString);

            var optionsBuilder = new DbContextOptionsBuilder<SmileContext>();
            optionsBuilder.UseSqlite(builder.ToString());
            options = optionsBuilder.Options;

            optionsBuilder = new DbContextOptionsBuilder<SmileContext>();
            optionsBuilder.UseSqlite(builder.ToString());
            migrateOptions = optionsBuilder.Options;
        }

        public void Setup()
        {
            using(var context = new SmileContext(options))
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    var mContext = new SmileContext(migrateOptions);
                    mContext.Database.Migrate();
                    mContext.SaveChanges();
                    mContext.Dispose();
                }
                context.Database.ExecuteSqlRaw("PRAGMA journal_mode=WAL");
                context.SaveChanges();
            }
        }

        public IUnitOfWork GetDbContext()
        {
            var context = new SmileContext(options);
            context.Database.SetCommandTimeout(60);
            return new UnitOfWork(context);
        }
    }
}