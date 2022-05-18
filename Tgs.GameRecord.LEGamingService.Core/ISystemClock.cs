namespace Tgs.GameRecord.LEGamingService.Core
{
    public interface ISystemClock
    {
        DateTimeOffset CurrentTime { get; }
    }
}
