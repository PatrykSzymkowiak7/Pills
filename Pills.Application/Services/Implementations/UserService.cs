using System.Security.Claims;
using Pills.Infrastructure.Services.Interfaces;

namespace Pills.Infrastructure.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string SystemUsername = "System";

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId
        {
            get
            {
                return _httpContextAccessor
                    .HttpContext?
                    .User?
                    .FindFirstValue(ClaimTypes.NameIdentifier) ?? SystemUsername;
            }
        }
    }
}
