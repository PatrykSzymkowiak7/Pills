using Microsoft.EntityFrameworkCore;
using Pills.Infrastructure;
using Pills.Infrastructure.BackgroundServices;
using Pills.Domain.Models;
using Pills.Infrastructure.Options;

namespace Pills.Infrastructure.BackgroundServices
{
    public class DailyReportBackgroundService : BackgroundService
    {
        private readonly ILogger<DailyReportBackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly FeatureFlags _featureFlags;
        public DailyReportBackgroundService(ILogger<DailyReportBackgroundService> logger, 
            IServiceScopeFactory scopeFactory, FeatureFlags featureFlags)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _featureFlags = featureFlags;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while(!stoppingToken.IsCancellationRequested)
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                        var yesterday = DateTime.UtcNow.Date.AddDays(-1);

                        var exists = await dbContext.DailyPillReport
                            .AnyAsync(r => r.Date == yesterday);

                        if(!exists)
                        {
                            var count = await dbContext.PillsTaken
                                .Where(p => p.Date.Date == yesterday)
                                .CountAsync();

                            dbContext.DailyPillReport.Add(new DailyPillReport
                            {
                                Date = yesterday,
                                TotalPillsTaken = count,
                                CreatedAt = DateTime.UtcNow
                            });

                            await dbContext.SaveChangesAsync(stoppingToken);
                        }
                    }

                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occured during DailyReportBackgroundService");
            }
        }
    }
}
