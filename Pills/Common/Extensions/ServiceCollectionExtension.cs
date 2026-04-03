using System.Threading.RateLimiting;
using Pills.Infrastructure;
using Pills.Infrastructure.BackgroundServices;
using Pills.Infrastructure.Common;
using Pills.Infrastructure.Common.HealthChecks;
using Pills.Infrastructure.Common.Interfaces;
using Pills.Infrastructure.Controllers.Filters;
using Pills.Infrastructure.Services.Implementations;
using Pills.Infrastructure.Services.Interfaces;

namespace Pills.Infrastructure.Common.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            #region Scoped

            services.AddScoped<IPillService, PillService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<AdminAuditFilter>();
            services.AddScoped<IReminderService, ReminderService>();

            #endregion

            #region Singleton

            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            #endregion

            #region Backgroundworkers

            services.AddHostedService<PillsStatsBackgroundService>();
            services.AddHostedService<DailyReportBackgroundService>();
            services.AddHostedService<ReminderBackgroundService>();
            services.AddHostedService<SoftDeleteCleanUpBackgroundService>();

            #endregion

            #region Others

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddHttpContextAccessor();
            services.AddMemoryCache();
            services.AddHealthChecks()
                .AddDbContextCheck<AppDbContext>("Database")
                .AddCheck<PillTypesHealthCheck>("Pill types")
                .AddCheck<AdminUserHealthCheck>("Admin user");
            services.AddRateLimiter(options =>
            {
                options.AddPolicy("login", context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 5,
                            Window = TimeSpan.FromMinutes(1)
                        }));
            });

            #endregion

            return services;
        }
    }
}
