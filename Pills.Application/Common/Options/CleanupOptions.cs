namespace Pills.Application.Common.Options
{
    // Options configurable in appsettings for SoftDeleteCleanupBackgroundService
    public class CleanupOptions
    {
        public static readonly string SectionName = "CleanupOptions";
        public int RetentionDays { get; set; }
        public int IntervalMinutes { get; set; }
    }
}
