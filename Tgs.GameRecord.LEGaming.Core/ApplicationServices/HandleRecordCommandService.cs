using System.Text.Json;
using Tgs.GameRounds.Work.Client;
using Tgs.GameRounds.Work.Common.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.LEGaming.Core.ApplicationServices;

internal class HandleRecordCommandService : ICommandService<HandleRecordCommand>
{
    private readonly IGameRoundsWorkClient _client;
    private readonly IUtensil _utensil;

    public HandleRecordCommandService(
        IUtensil utensil,
        IGameRoundsWorkClient client)
    {
        _utensil = utensil ?? throw new ArgumentNullException(nameof(utensil));
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask ExecuteAsync(HandleRecordCommand command, CancellationToken cancellationToken = default)
    {
        var gameProviderId = await _utensil
            .GetGameProviderIdAsync(
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var gameProviderCode = _utensil
            .GetGameProviderCode();

        var playerModel = await _utensil
            .GetPlayerAsync(
                playerCode: command.Record.PlayerCode,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
        var gameId = await _utensil
            .GetGameIdAsync(
            gameProviderId: gameProviderId,
            gameExternalId: command.Record.ExternalGameId,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);


        var gameProviderConfigurationId = await _utensil
            .GetBrandGameProviderConfigurationIdAsync(
            brandId: playerModel.BrandId,
            gameProviderId: gameProviderId,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var clientStatus = (command.Record.WinLoss.ToDecimal().GetValueOrDefault(0) > 0) ? RoundClientStatus.Win : RoundClientStatus.Loss;
        
        await _client
            .RoundExchangeRateBiddingAsync(
            gameProviderId: gameProviderId,
            gameProviderCode: gameProviderCode,
            externalRoundId: command.Record.ExternalRoundId,
            gameId: gameId!,
            externalGameId: command.Record.ExternalGameId,
            playerCode: command.Record.PlayerCode,
            walletId: playerModel.DefaultWalletId,
            currencyCode: playerModel.DefaultWalletCurrencyCode,
            brandId: playerModel.BrandId,
            gameProviderConfigurationId: gameProviderConfigurationId,
            status: RoundStatus.Closed,
            clientStatus: clientStatus,
            externalCreatedOn: command.Record.ExternalCreatedOn.ToDateTimeOffset(),
            externalClosedOn: command.Record.ExternalClosedOn.ToDateTimeOffset(),
            bet: command.Record.Bet.ToDecimal().GetValueOrDefault(0),
            validBet: command.Record.ValidBet.ToDecimal().GetValueOrDefault(0),
            settle: command.Record.Settle.ToDecimal().GetValueOrDefault(0),
            commission: command.Record.Commission.ToDecimal().GetValueOrDefault(0),
            turnover: command.Record.Turnover.ToDecimal().GetValueOrDefault(0),
            playerWinLoss: command.Record.WinLoss.ToDecimal().GetValueOrDefault(0),
            additionalData: command.Record.AdditionalData,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}
