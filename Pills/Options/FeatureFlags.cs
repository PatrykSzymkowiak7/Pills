namespace Pills.Options
{
    public class FeatureFlags
    {
        public static readonly string SectionName = "Features";
        public bool EnablePillTypeDelete { get; set; }
        public bool EnablePillStatistics { get; set; }
        public bool EnableSoftDelete { get; set; }
    }
}
