
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pills.Infrastructure;

using Pills.Infrastructure.BackgroundServices;

namespace Pills.Infrastructure.BackgroundServices
{
    public class PillsStatsBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<PillsStatsBackgroundService> _logger;

        public PillsStatsBackgroundService(IServiceScopeFactory scopeFactory, 
            ILogger<PillsStatsBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("PillsStatsBackgroundService started");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                        var count = await dbContext.PillsTaken.CountAsync(stoppingToken);

                        _logger.LogInformation("Total pills taken: {count}", count);

                        await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Background service failed");
            }
        }
    }
}
