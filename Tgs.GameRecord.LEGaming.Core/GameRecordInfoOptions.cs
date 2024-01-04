namespace Tgs.GameRecord.LEGaming.Core;

public record GameRecordInfoOptions
{
    public string GameProviderCode { get; set; } = default!;
    public string GameProviderConfiguration { get; set; } = default!;
    public string HangfirePrefix { get; set; } = "gamerecord-ly-hangfire:";
    public string HangfireLocking { get; set; } = "gamerecord-ly-locking:";
    public double IntervalMinutes { get; set; } = 20d;
    public double IntervalMinutesMax { get; set; } = 30d;
    public double IntervalMinutesMinimum { get; set; } = 10d;
}
