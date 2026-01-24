using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Pills.Identity;

namespace Pills.Common.HealthChecks
{
    public class AdminUserHealthCheck : IHealthCheck
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly string AdminEmail = "admin@pills.app";

        public AdminUserHealthCheck(AppDbContext appDbContext, 
            UserManager<ApplicationUser> userManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, 
            CancellationToken cancellationToken = default)
        {
            var adminUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == AdminEmail);

            if (adminUser == null)
                return HealthCheckResult.Unhealthy("Admin user not found");

            var isInRole = await _userManager.IsInRoleAsync(adminUser, UserRoles.Admin);

            if (!isInRole)
                return HealthCheckResult.Unhealthy("Admin user is not in role admin");

            return HealthCheckResult.Healthy("Admin user exists");
        }
    }
}
