using JobHunterAPI.Data;
using JobHunterAPI.Middleware;
using Microsoft.EntityFrameworkCore;

namespace JobHunterAPI.Seeders
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());
            if (!context.ApiKeys.Any())
            {
                context.ApiKeys.Add(new ApiKey
                {
                    Key = "your-test-api-key",
                    ExpiresAt = DateTime.UtcNow.AddMonths(6)
                });
                context.SaveChanges();
            }
        }
    }

}
