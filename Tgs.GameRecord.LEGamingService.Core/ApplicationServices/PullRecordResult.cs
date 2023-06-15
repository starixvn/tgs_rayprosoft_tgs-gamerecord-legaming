namespace Tgs.GameRecord.LEGamingService.Core.ApplicationServices;

public record PullRecordResult(
    bool Success,
    DateTimeOffset? LastGameRecordTime);
