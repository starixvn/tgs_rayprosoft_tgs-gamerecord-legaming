using Microsoft.Extensions.Logging;
using Tgs.GameProviders.Client;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.LEGamingService.Core.ApplicationServices;

internal class PullRecordGetService : IGetService<PullRecordQuery, PullRecordResult>
{
    private readonly IGameRecordListClient _client;
    private readonly ILogger<PullRecordGetService> _logger;
    private readonly ICommandService<ScheduleHandleRecordCommand> _service;

    public PullRecordGetService(
        ILogger<PullRecordGetService> logger,
        IGameRecordListClient client,
        ICommandService<ScheduleHandleRecordCommand> service)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    public async ValueTask<PullRecordResult> GetAsync(PullRecordQuery query, CancellationToken cancellationToken = default)
    {
        var response = await _client.LYGameRecordListAsync(
            brandCode: query.BrandCode,
            currencyCode: query.CurrencyCode,
            startTime: query.StartTime,
            endTime: query.EndTime,
            cancellationToken: cancellationToken).ConfigureAwait(false);

        if (response == null || !response.Success)
        {
            return new(Success: false, LastGameRecordTime: null);
        }

        var datas = response.Datas ?? Enumerable.Empty<GameRecordModle>();

        var lastGameRecordTime = default(DateTimeOffset?);

        foreach (var data in datas)
        {
            try
            {
                await _service
                 .ExecuteAsync(
                     command: new ScheduleHandleRecordCommand(data),
                     cancellationToken: cancellationToken)
                 .ConfigureAwait(false);

                lastGameRecordTime = data.ExternalClosedOn.HasValue ? data.ExternalClosedOn : data.ExternalCreatedOn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "遊戲紀錄處理發生例外.({gameRecord})", data);
            }
        };

        return new(Success: true, LastGameRecordTime: lastGameRecordTime);
    }
}
