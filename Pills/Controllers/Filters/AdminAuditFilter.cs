using Microsoft.AspNetCore.Mvc.Filters;

namespace Pills.Controllers.Filters
{
    public class AdminAuditFilter : IActionFilter
    {
        private readonly ILogger<AdminAuditFilter> _logger;

        public AdminAuditFilter(ILogger<AdminAuditFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User.Identity?.Name;
            var action = context.ActionDescriptor.DisplayName;

            _logger.LogInformation($"User {user} calls {action}", user, action);
        }
    }
}
