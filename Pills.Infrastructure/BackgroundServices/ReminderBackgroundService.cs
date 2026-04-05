
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pills.Infrastructure;
using Pills.Infrastructure.BackgroundServices;
using Pills.Infrastructure.Identity;

namespace Pills.Infrastructure.BackgroundServices
{
    public class ReminderBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ReminderBackgroundService> _logger;

        public ReminderBackgroundService(IServiceScopeFactory scopeFactory, 
            ILogger<ReminderBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ReminderBackgroundService start");

            while(stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using(var scope = _scopeFactory.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                        var now = DateTime.UtcNow;
                        var today = now.Date;

                        var dueReminders = await dbContext.Reminders
                            .Where(r =>
                                r.IsEnabled &&
                                r.TimeOfDay <= now.TimeOfDay &&
                                (r.LastSentAt == null || r.LastSentAt.Value.Date < today))
                            .ToListAsync();

                        foreach(var reminder in dueReminders)
                        {
                            _logger.LogInformation(
                                "Reminder for {User}: {Msg}",
                                reminder.UserId,
                                reminder.Message);

                            var user = await userManager.FindByIdAsync(reminder.UserId);

                            // placeholder for send mail - create SMTP server first

                            reminder.LastSentAt = now;
                        }

                        if (dueReminders.Any())
                            await dbContext.SaveChangesAsync(stoppingToken);
                    }
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "An error occured during ReminderBackgroundService");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
