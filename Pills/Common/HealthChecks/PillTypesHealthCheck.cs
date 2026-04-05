using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Pills.Infrastructure;

namespace Pills.Web.Common.HealthChecks
{
    public class PillTypesHealthCheck : IHealthCheck
    {
        private readonly AppDbContext _appDbContext;

        public PillTypesHealthCheck(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, 
            CancellationToken cancellationToken = default)
        {
            var any = await _appDbContext.PillsTypes.AnyAsync();

            if(!any)
            {
                return HealthCheckResult.Unhealthy("There is no pill types in database");
            }

            return HealthCheckResult.Healthy("Pill types exist");
        }
    }
}
