using Pills.BackgroundServices;
using Pills.Common.HealthChecks;
using Pills.Common.Interfaces;
using Pills.Controllers.Filters;
using Pills.Services.Implementations;
using Pills.Services.Interfaces;
using System.Threading.RateLimiting;

namespace Pills.Common.Extensions
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
