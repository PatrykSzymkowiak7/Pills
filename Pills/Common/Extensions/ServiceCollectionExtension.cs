using Pills.Common.HealthChecks;
using Pills.Common.Interfaces;
using Pills.Controllers.Filters;
using Pills.Services.Implementations;
using Pills.Services.Interfaces;

namespace Pills.Common.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // scoped
            services.AddScoped<IPillService, PillService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<AdminAuditFilter>();

            // singletons
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            // others
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddHttpContextAccessor();
            services.AddMemoryCache();
            services.AddHealthChecks()
                .AddDbContextCheck<AppDbContext>("Database")
                .AddCheck<PillTypesHealthCheck>("Pill types")
                .AddCheck<AdminUserHealthCheck>("Admin user");


            return services;
        }
    }
}
