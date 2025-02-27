using Microsoft.EntityFrameworkCore;
using multitenant_app.Context;

namespace multitenant_app.Services
{
    public class UserDatabaseService
    {
        private readonly IConfiguration _configuration;

        public UserDatabaseService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnectionString(string dbName)
        {
            return _configuration.GetConnectionString("DataBaseContextUser").Replace("{DatabaseName}", dbName);
        }

        public async Task CreateUserDatabase(string dbName)
        {
            var connectionString = GetConnectionString(dbName);

            var optionBuilder = new DbContextOptionsBuilder<DatabaseContextUser>();
            optionBuilder.UseSqlServer(connectionString);

            using (var context = new DatabaseContextUser(optionBuilder.Options, connectionString))
            {
                await context.Database.MigrateAsync();
            }
        }
        public async Task UpdateDatabase(string dbName)
        {
            var connectionString = GetConnectionString(dbName);

            var optionBuilder = new DbContextOptionsBuilder<DatabaseContextUser>();
            optionBuilder.UseSqlServer(connectionString);

            using (var context = new DatabaseContextUser(optionBuilder.Options, connectionString))
            {
                var pendingMigrations = context.Database.GetPendingMigrations();
                if (pendingMigrations.Any())
                {
                    await context.Database.MigrateAsync();
                }
            }
        }
    }
}
