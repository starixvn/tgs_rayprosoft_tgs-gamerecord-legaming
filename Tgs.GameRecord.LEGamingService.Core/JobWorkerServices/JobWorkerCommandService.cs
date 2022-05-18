using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tgs.GameRecord.LEGamingService.Core.ApplicationServices;
using Tgs.GameRecordSchedules.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.LEGamingService.Core.JobWorkerServices
{
    internal class JobWorkerCommandService : ICommandService<JobWorkerCommand>
    {
        private readonly GameRecordInfoOptions _gameRecordInfo;
        private readonly IGameRecordSchedule _gameRecordScheduleService;
        private readonly IGetService<GameRecordQuery, GameRecordResult> _getService;
        private readonly ILogger<JobWorkerCommandService> _logger;
        private readonly ILEGaming _service;
        private readonly ISystemClock _systemClock;

        public JobWorkerCommandService(
            IOptions<GameRecordInfoOptions> options,
            ILEGaming service,
            IGameRecordSchedule gameRecordScheduleService,
            ISystemClock systemClock,
            ILogger<JobWorkerCommandService> logger,
            IGetService<GameRecordQuery, GameRecordResult> getService)
        {
            _gameRecordInfo = options.Value;
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _gameRecordScheduleService = gameRecordScheduleService ?? throw new ArgumentNullException(nameof(gameRecordScheduleService));
            _systemClock = systemClock ?? throw new ArgumentNullException(nameof(systemClock));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _getService = getService ?? throw new ArgumentNullException(nameof(getService));
        }

        public async ValueTask ExecuteAsync(JobWorkerCommand command, CancellationToken cancellationToken = default)
        {
            var configuration = await _service.GetSettingConfigurationAsync().ConfigureAwait(false);

            var gameProviderId = await _service.GetGameProviderIdAsync().ConfigureAwait(false);

            var brandMerchants = configuration.BrandMerchants.Where(x => x.Status);
            foreach (var brand in brandMerchants)
            {
                foreach (var agent in brand.AgentMappers.Where(x => x.Status))
                {
                    try
                    {
                        var sysTime = _systemClock.CurrentTime;

                        var request = new GameRecordScheduleGetRequest()
                        {
                            GameProviderCode = _gameRecordInfo.GameProviderCode,
                            BrandCode = brand.BrandCode,
                            GameProviderAgentId = agent.AgentId,
                            CurrencyCode = agent.CurrencyCode,
                        };

                        var gameProviderSchedule =
                            await _gameRecordScheduleService.GetAsync(
                                brand.BrandCode,
                                agent.AgentId,
                                agent.CurrencyCode,
                                cancellationToken).ConfigureAwait(false)
                                ?? await _gameRecordScheduleService.InitAsync(
                                    gameProviderId,
                                    brand.BrandCode,
                                    agent.AgentId,
                                    agent.CurrencyCode,
                                    cancellationToken).ConfigureAwait(false);

                        while (gameProviderSchedule != null)
                        {
                            var config = _gameRecordScheduleService.GetConfig(gameProviderSchedule);
                            if (config.EndTime >= sysTime)
                            {
                                break;
                            }

                            var result = await _getService.GetAsync(new GameRecordQuery()
                            {
                                BrandCode = brand.BrandCode,
                                CurrencyCode = agent.CurrencyCode,
                                StartTime = config.StartTime,
                                EndTime = config.EndTime
                            }, cancellationToken).ConfigureAwait(false);
                            if (result.Success)
                            {
                                gameProviderSchedule = await _gameRecordScheduleService.UpdateAsync(gameProviderSchedule, result.LastGameRecorTime, sysTime, cancellationToken).ConfigureAwait(false);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"發生例外");
                    }
                }
            }
        }
    }
}
