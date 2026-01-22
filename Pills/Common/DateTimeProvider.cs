namespace Pills.Common
{
    public class DateTimeProvider :IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
