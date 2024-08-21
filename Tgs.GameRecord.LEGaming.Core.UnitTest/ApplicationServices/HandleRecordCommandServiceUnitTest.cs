using System;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Tgs.GameRecord.Handle.Client;
using Tgs.GameRecord.LEGaming.Core.ApplicationServices;
using Valhalla.MessageQueue;

namespace Tgs.GameRecord.LEGaming.Core.UnitTest.ApplicationServices;

public class HandleRecordCommandServiceUnitTest
{
	[Fact]
	public async Task Loss()
	{
		var faceLogger = NullLogger<HandleRecordCommandService>.Instance;
		var fakeGameRecordInfoOptions = Options.Create(new GameRecordInfoOptions
		{
			GameProviderCode = "LY",
			GameProviderConfiguration = "LYConfiguration",
			HangfirePrefix = "gamerecord-ly-hangfire:",
			HangfireLocking = "gamerecord-ly-locking:",
			IntervalMinutes = 5,
			IntervalMinutesMax = 30,
			IntervalMinutesMinimum = 3
		});
		var fakePolicySettingsOptions = Options.Create(new PolicySettingsOptions
		{
			RetryCount = 1,
			RetrySeconds = 1,
		});
		var fakeGameRecordHandleClient = Substitute.For<IGameRecordHandleClient>();

		var command = new HandleRecordCommand(
			Info: new GameInterface.LEGaming.Protos.GameRecordInfo
			{
				PlayerCode = "4ukkm20cwny2",
				ExternalBetId = "888-1692114604-19026124-1",
				ExternalGameId = "740",
				ExternalCreatedOn = DateTimeOffset.Parse("2023-08-15T23:50:04+08:00").ToTimestamp(),
				ExternalClosedOn = DateTimeOffset.Parse("2023-08-15T23:50:52+08:00").ToTimestamp(),
				Bet = 7.50m,
				ValidBet = 7.50m,
				Commission = 0.0m,
				Settle = 0m,
				WinLoss = -7.50m,
				AdditionalData = "{\"GameID\":\"888-1692114604-19026124-1\",\"Accounts\":\"72843_4ukkm20cwny2\",\"ServerID\":7400,\"KindID\":740,\"TableID\":148000003,\"ChairID\":1,\"UserCount\":2,\"CellScore\":7.5,\"AllBet\":7.5,\"Profit\":-7.5,\"Revenue\":0,\"GameStartTime\":\"2023-08-15T23:50:04+08:00\",\"GameEndTime\":\"2023-08-15T23:50:52+08:00\",\"CardValue\":\"040406070809090916161111111122,0405060707071616151515020202212627,0\",\"ChannelID\":72843,\"LineCode\":\"72843_King8\"}"
			});

		var sut = new HandleRecordCommandService(
			faceLogger,
			fakeGameRecordInfoOptions,
			fakePolicySettingsOptions,
			fakeGameRecordHandleClient);

		await sut.ExecuteAsync(command);

		_ = fakeGameRecordHandleClient
			.Received(1)
			.HandleAsync(
			source: Arg.Is(new Handle.Common.ViewModels.GameRecordHandleViewModel
			{
				GameProviderCode = fakeGameRecordInfoOptions.Value.GameProviderCode,
				PlayerCode = command.Info.PlayerCode,
				ExternalRoundId = command.Info.ExternalBetId,
				ExternalBetId = command.Info.ExternalBetId,
				ExternalGameId = command.Info.ExternalGameId,
				ExternalCreatedOn = command.Info.ExternalCreatedOn.ToDateTimeOffset(),
				ExternalClosedOn = command.Info.ExternalClosedOn.ToDateTimeOffset(),
				ExternalPayoffOn = command.Info.ExternalClosedOn.ToDateTimeOffset(),
				ExternalResettleOn = DateTimeOffset.UnixEpoch,
				BetState = Handle.Common.Protos.BetState.Loss,
				BetType = Handle.Common.Protos.BetType.Normal,
				FreeGameState = Handle.Common.Protos.FreeGameState.None,
				GambleState = Handle.Common.Protos.GambleState.None,
				CashoutState = Handle.Common.Protos.CashoutState.None,
				ResettleState = Handle.Common.Protos.ResettleState.None,
				JackpotState = Handle.Common.Protos.JackpotState.None,
				Bet = 7.50m,
				ValidBet = 7.50m,
				Settle = 0.0M,
				Commission = 0.0M,
				Turnover = 7.50m,
				WinLoss = -7.50m,
				Jackpot = 0.0M,
				AdditionalData = command.Info.AdditionalData
			}),
			cancellationToken: Arg.Any<CancellationToken>());
	}

	[Fact]
	public async Task Win()
	{
		var faceLogger = NullLogger<HandleRecordCommandService>.Instance;
		var fakeGameRecordInfoOptions = Options.Create(new GameRecordInfoOptions
		{
			GameProviderCode = "LY",
			GameProviderConfiguration = "LYConfiguration",
			HangfirePrefix = "gamerecord-ly-hangfire:",
			HangfireLocking = "gamerecord-ly-locking:",
			IntervalMinutes = 5,
			IntervalMinutesMax = 30,
			IntervalMinutesMinimum = 3
		});
		var fakePolicySettingsOptions = Options.Create(new PolicySettingsOptions
		{
			RetryCount = 1,
			RetrySeconds = 1,
		});
		var fakeGameRecordHandleClient = Substitute.For<IGameRecordHandleClient>();

		var command = new HandleRecordCommand(
			Info: new GameInterface.LEGaming.Protos.GameRecordInfo
			{
				PlayerCode = "4ukkm20cwny2",
				ExternalBetId = "888-1692113797-19006265-2",
				ExternalGameId = "740",
				ExternalCreatedOn = DateTimeOffset.Parse("2023-08-15T23:36:37+08:00").ToTimestamp(),
				ExternalClosedOn = DateTimeOffset.Parse("2023-08-15T23:37:30+08:00").ToTimestamp(),
				Bet = 16.5m,
				ValidBet = 16.5m,
				Commission = 0.0m,
				Settle = 32.18m,
				WinLoss = 15.68m,
				AdditionalData = "{\"GameID\":\"888-1692113797-19006265-2\",\"Accounts\":\"72843_4ukkm20cwny2\",\"ServerID\":7400,\"KindID\":740,\"TableID\":148000019,\"ChairID\":2,\"UserCount\":2,\"CellScore\":16.5,\"AllBet\":16.5,\"Profit\":15.68,\"Revenue\":0.82,\"GameStartTime\":\"2023-08-15T23:36:37+08:00\",\"GameEndTime\":\"2023-08-15T23:37:30+08:00\",\"CardValue\":\"0306060808080914141516161616252326,0304040405060708020202010101212228,0\",\"ChannelID\":72843,\"LineCode\":\"72843_King8\"}"
			});

		var sut = new HandleRecordCommandService(
			faceLogger,
			fakeGameRecordInfoOptions,
			fakePolicySettingsOptions,
			fakeGameRecordHandleClient);

		await sut.ExecuteAsync(command);

		_ = fakeGameRecordHandleClient
			.Received(1)
			.HandleAsync(
			source: Arg.Is(new Handle.Common.ViewModels.GameRecordHandleViewModel
			{
				GameProviderCode = fakeGameRecordInfoOptions.Value.GameProviderCode,
				PlayerCode = command.Info.PlayerCode,
				ExternalRoundId = command.Info.ExternalBetId,
				ExternalBetId = command.Info.ExternalBetId,
				ExternalGameId = command.Info.ExternalGameId,
				ExternalCreatedOn = command.Info.ExternalCreatedOn.ToDateTimeOffset(),
				ExternalClosedOn = command.Info.ExternalClosedOn.ToDateTimeOffset(),
				ExternalPayoffOn = command.Info.ExternalClosedOn.ToDateTimeOffset(),
				ExternalResettleOn = DateTimeOffset.UnixEpoch,
				BetState = Handle.Common.Protos.BetState.Win,
				BetType = Handle.Common.Protos.BetType.Normal,
				FreeGameState = Handle.Common.Protos.FreeGameState.None,
				GambleState = Handle.Common.Protos.GambleState.None,
				CashoutState = Handle.Common.Protos.CashoutState.None,
				ResettleState = Handle.Common.Protos.ResettleState.None,
				JackpotState = Handle.Common.Protos.JackpotState.None,
				Bet = 16.5m,
				ValidBet = 16.5m,
				Settle = 32.18m,
				Commission = 0.0M,
				Turnover = 16.5m,
				WinLoss = 15.68m,
				Jackpot = 0.0M,
				AdditionalData = command.Info.AdditionalData
			}),
			cancellationToken: Arg.Any<CancellationToken>());
	}
}
