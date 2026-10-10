using ElAmal.Core.Contract.Repository;
using Microsoft.EntityFrameworkCore;

namespace ElAmal.Infrastructure.Data
{
    public class ElAmalContextInitializer (ElAmalDbContext _dbContext) : IElAmalContextInitializer
    {   
        public async Task UpdateDatabaseAsync()
        {
            var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();

            if (pendingMigrations.Any())
                await _dbContext.Database.MigrateAsync();
        }

        public Task SeedAsync()
        {
            throw new NotImplementedException();
        }

    }
}
