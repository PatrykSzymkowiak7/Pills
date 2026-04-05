using System.Threading.RateLimiting;
using Pills.Infrastructure;
using Pills.Infrastructure.BackgroundServices;
using Pills.Application.Services;
using Pills.Application.Interfaces;
using Pills.Infrastructure.Repositories;
using Pills.Web.Common;
using Pills.Web.Common.HealthChecks;
using Pills.Web.Services;
using Pills.Application.Common.Filters;

namespace Pills.Web.Common.Extensions
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
            services.AddScoped<IPillRepository, PillRepository>();
            services.AddScoped<IReminderRepository, ReminderRepository>();

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
