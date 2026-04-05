using Pills.Application.Interfaces;

namespace Pills.Web.Common
{
    public class DateTimeProvider :IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
