namespace Tgs.GameRecord.LEGamingService.Core
{
    internal class SystemClock : ISystemClock
    {
        public DateTimeOffset CurrentTime => DateTimeOffset.UtcNow;
    }
}
