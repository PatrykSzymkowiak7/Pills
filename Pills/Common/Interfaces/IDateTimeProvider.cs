namespace Pills.Common.Interfaces
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }
}
