namespace Tgs.GameRecord.LEGaming.Core;

public record GameRecordInfoOptions
{
    public string GameProviderCode { get; set; } = default!;
    public string GameProviderConfiguration { get; set; } = default!;
    public string GameProviderHangfire { get; set; } = default!;
    public double IntervalMinutes { get; set; } = 20d;
    public double IntervalMinutesMax { get; set; } = 30d;
    public double IntervalMinutesMinimum { get; set; } = 10d;
}
