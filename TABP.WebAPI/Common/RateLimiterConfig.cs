namespace TABP.WebAPI.Common;

public class RateLimiterConfig
{
    public int TokenLimit { get; set; }
    public int TokensPerPeriod { get; set; }
    public int QueueLimit { get; set; }
    public bool AutoReplenishment { get; set; }
    public int ReplenishmentPeriod { get; set; }
}