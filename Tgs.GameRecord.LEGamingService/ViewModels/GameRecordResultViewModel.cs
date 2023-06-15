namespace Tgs.GameRecord.LEGamingService.ViewModels;

public record GameRecordResultViewModel
{
    public bool Success { get; init; }
    public DateTimeOffset? LastGameRecordTime { get; init; }
}
