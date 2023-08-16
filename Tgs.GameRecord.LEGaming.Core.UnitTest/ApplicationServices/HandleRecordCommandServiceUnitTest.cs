using System;
using System.Threading;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Options;
using NSubstitute;
using Tgs.GameInterface.LEGaming.Protos;
using Tgs.GameRecord.Handle.Client;
using Tgs.GameRecord.Handle.Common.Protos;
using Tgs.GameRecord.LEGaming.Core.ApplicationServices;

namespace Tgs.GameRecord.LEGaming.Core.UnitTest.ApplicationServices;

public class HandleRecordCommandServiceUnitTest
{
    [Fact]
    public async void Loss()
    {
        var gameRecordInfo = Options.Create(new GameRecordInfoOptions
        {
            GameProviderCode = "LY",
            GameProviderConfiguration = "LYConfiguration",
            GameProviderHangfire = "ly-schedule:hangfire:",
            IntervalMinutes = 5,
            IntervalMinutesMax = 30,
            IntervalMinutesMinimum = 3
        });
        var fakeClient = Substitute.For<IGameRecordHandleClient>();

        var command = new HandleRecordCommand(
            Record: new GameRecordInfo
            {
                ExternalRoundId = "888-1692114604-19026124-1",
                ExternalGameId = "740",
                PlayerCode = "4ukkm20cwny2",
                ExternalCreatedOn = DateTimeOffset.Parse("2023-08-15T23:50:04+08:00").ToTimestamp(),
                ExternalClosedOn = DateTimeOffset.Parse("2023-08-15T23:50:52+08:00").ToTimestamp(),
                Bet = 7.50m,
                ValidBet = 7.50m,
                Commission = 0.0m,
                Turnover = 7.50m,
                Settle = 0m,
                WinLoss = -7.50m,
                AdditionalData = "{\"GameID\":\"888-1692114604-19026124-1\",\"Accounts\":\"72843_4ukkm20cwny2\",\"ServerID\":7400,\"KindID\":740,\"TableID\":148000003,\"ChairID\":1,\"UserCount\":2,\"CellScore\":7.5,\"AllBet\":7.5,\"Profit\":-7.5,\"Revenue\":0,\"GameStartTime\":\"2023-08-15T23:50:04+08:00\",\"GameEndTime\":\"2023-08-15T23:50:52+08:00\",\"CardValue\":\"040406070809090916161111111122,0405060707071616151515020202212627,0\",\"ChannelID\":72843,\"LineCode\":\"72843_King8\"}"
            });

        var service = new HandleRecordCommandService(
            gameRecordInfo,
            fakeClient);

        await service
            .ExecuteAsync(
            command)
            .ConfigureAwait(false);

        await fakeClient
            .Received(1)
            .MessageAsync(
            gameProviderCode: Arg.Is(gameRecordInfo.Value.GameProviderCode),
            externalRoundId: Arg.Is(command.Record.ExternalRoundId),
            externalGameId: Arg.Is(command.Record.ExternalGameId.ToString()),
            playerCode: Arg.Is(command.Record.PlayerCode),
            status: Arg.Is(GameRecordStatus.Closed),
            clientStatus: Arg.Is(GameRecordClientStatus.Loss),
            externalCreatedOn: Arg.Is(command.Record.ExternalCreatedOn.ToDateTimeOffset()),
            externalClosedOn: Arg.Is(command.Record.ExternalClosedOn.ToDateTimeOffset()),
            bet: Arg.Is(7.50m),
            validBet: Arg.Is(7.50m),
            settle: Arg.Is(0.0m),
            commission: Arg.Is(0.0m),
            turnover: Arg.Is(7.50m),
            playerWinLoss: Arg.Is(-7.50m),
            additionalData: Arg.Is(command.Record.AdditionalData),
            cancellationToken: Arg.Any<CancellationToken>())
            .ConfigureAwait(false);
    }

    [Fact]
    public async void Win()
    {
        var gameRecordInfo = Options.Create(new GameRecordInfoOptions
        {
            GameProviderCode = "LY",
            GameProviderConfiguration = "LYConfiguration",
            GameProviderHangfire = "ly-schedule:hangfire:",
            IntervalMinutes = 5,
            IntervalMinutesMax = 30,
            IntervalMinutesMinimum = 3
        });
        var fakeClient = Substitute.For<IGameRecordHandleClient>();

        var command = new HandleRecordCommand(
            Record: new GameRecordInfo
            {
                ExternalRoundId = "888-1692113797-19006265-2",
                ExternalGameId = "740",
                PlayerCode = "4ukkm20cwny2",
                ExternalCreatedOn = DateTimeOffset.Parse("2023-08-15T23:36:37+08:00").ToTimestamp(),
                ExternalClosedOn = DateTimeOffset.Parse("2023-08-15T23:37:30+08:00").ToTimestamp(),
                Bet = 16.5m,
                ValidBet = 16.5m,
                Commission = 0.0m,
                Turnover = 16.5m,
                Settle = 32.18m,
                WinLoss = 15.68m,
                AdditionalData = "{\"GameID\":\"888-1692113797-19006265-2\",\"Accounts\":\"72843_4ukkm20cwny2\",\"ServerID\":7400,\"KindID\":740,\"TableID\":148000019,\"ChairID\":2,\"UserCount\":2,\"CellScore\":16.5,\"AllBet\":16.5,\"Profit\":15.68,\"Revenue\":0.82,\"GameStartTime\":\"2023-08-15T23:36:37+08:00\",\"GameEndTime\":\"2023-08-15T23:37:30+08:00\",\"CardValue\":\"0306060808080914141516161616252326,0304040405060708020202010101212228,0\",\"ChannelID\":72843,\"LineCode\":\"72843_King8\"}"
            });

        var service = new HandleRecordCommandService(
            gameRecordInfo,
            fakeClient);

        await service
            .ExecuteAsync(
            command)
            .ConfigureAwait(false);

        await fakeClient
            .Received(1)
            .MessageAsync(
            gameProviderCode: Arg.Is(gameRecordInfo.Value.GameProviderCode),
            externalRoundId: Arg.Is(command.Record.ExternalRoundId),
            externalGameId: Arg.Is(command.Record.ExternalGameId.ToString()),
            playerCode: Arg.Is(command.Record.PlayerCode),
            status: Arg.Is(GameRecordStatus.Closed),
            clientStatus: Arg.Is(GameRecordClientStatus.Win),
            externalCreatedOn: Arg.Is(command.Record.ExternalCreatedOn.ToDateTimeOffset()),
            externalClosedOn: Arg.Is(command.Record.ExternalClosedOn.ToDateTimeOffset()),
            bet: Arg.Is(16.5m),
            validBet: Arg.Is(16.5m),
            settle: Arg.Is(32.18m),
            commission: Arg.Is(0.0m),
            turnover: Arg.Is(16.5m),
            playerWinLoss: Arg.Is(15.68m),
            additionalData: Arg.Is(command.Record.AdditionalData),
            cancellationToken: Arg.Any<CancellationToken>())
            .ConfigureAwait(false);
    }
}
