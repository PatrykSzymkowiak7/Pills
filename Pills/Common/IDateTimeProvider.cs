namespace Pills.Common
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }
}
