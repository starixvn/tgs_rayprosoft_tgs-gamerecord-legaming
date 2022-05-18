namespace Tgs.GameRecord.LEGamingService
{
    public record LEGamingMessageModel
    {
        public string GameProviderCode { get; init; } = default!;
        public DateTimeOffset SendDateTime { get; init; }
    }
}
