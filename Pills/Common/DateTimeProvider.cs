using Pills.Infrastructure.Common.Interfaces;

namespace Pills.Infrastructure.Common
{
    public class DateTimeProvider :IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
