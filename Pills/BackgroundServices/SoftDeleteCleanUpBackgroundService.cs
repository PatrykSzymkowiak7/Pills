
using Microsoft.EntityFrameworkCore;
using Pills.Options;

namespace Pills.BackgroundServices
{
    public class SoftDeleteCleanUpBackgroundService : BackgroundService
    {
        private readonly ILogger<SoftDeleteCleanUpBackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly CleanupOptions _cleanupOptions;
        private readonly FeatureFlags _featureFlags;

        public SoftDeleteCleanUpBackgroundService(ILogger<SoftDeleteCleanUpBackgroundService> logger,
            IServiceScopeFactory scopeFactory, CleanupOptions cleanupOptions, FeatureFlags featureFlags)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _cleanupOptions = cleanupOptions;
            _featureFlags = featureFlags;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("SoftDeleteCleanUpBackgroundService start");

            try
            {
                while(!stoppingToken.IsCancellationRequested)
                {
                    if (_featureFlags.EnableSoftDelete)
                    {
                        using (var scope = _scopeFactory.CreateScope())
                        {
                            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                            dbContext.IgnoreSoftDelete = true;

                            var treshHold = DateTime.UtcNow.AddDays(-_cleanupOptions.RetentionDays);

                            var pillsTakenToDelete = await dbContext.PillsTaken
                                .Where(pt => pt.DeletedAt <= treshHold)
                                .ToListAsync(stoppingToken);

                            dbContext.RemoveRange(pillsTakenToDelete);

                            var deleted = pillsTakenToDelete.Count;

                            if (_featureFlags.EnablePillTypeDelete)
                            {
                                var pillsTypesToDelete = await dbContext.PillsTypes
                                    .Where(pt => pt.DeletedAt <= treshHold)
                                    .ToListAsync(stoppingToken);

                                dbContext.RemoveRange(pillsTypesToDelete);

                                deleted += pillsTypesToDelete.Count;
                            }

                            if (deleted > 0)
                            {
                                await dbContext.SaveChangesAsync(stoppingToken);
                                _logger.LogInformation("SoftDeleteCleanUpBackgroundService removed {Count} records", deleted);
                            }
                        }
                    }

                    await Task.Delay(TimeSpan.FromMinutes(_cleanupOptions.IntervalMinutes), stoppingToken);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occured during SoftDeleteCleanUpBackgroundservice execution");
            }
        }
    }
}
