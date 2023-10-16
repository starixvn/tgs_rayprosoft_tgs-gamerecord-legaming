using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Tgs.GameInterface.LEGaming.Client;
using Tgs.GameInterface.LEGaming.Protos;
using Tgs.GameRecord.LEGaming.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.LEGaming.Core.UnitTest.ApplicationServices;

public class PullRecordCommandServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeLogger = NullLogger<PullRecordCommandService>.Instance;
        var fakeClient = Substitute.For<IGameInterfaceLEGamingClient>();
        var fakeService = Substitute.For<ICommandService<HandleRecordCommand>>();

        var command = new PullRecordCommand(
            ApiUrl: new("http://localhost/"),
            Deskey: "deskey",
            Md5key: "md5key",
            AgentId: "agentId",
            StartTime: DateTimeOffset.UtcNow.AddMinutes(-10),
            EndTime: DateTimeOffset.UtcNow);

        _ = fakeClient
            .GameWagerAsync(
            apiUrl: Arg.Is(command.ApiUrl),
            deskey: Arg.Is(command.Deskey),
            md5key: Arg.Is(command.Md5key),
            agentId: Arg.Is(command.AgentId),
            startTime: Arg.Is(command.StartTime),
            endTime: Arg.Is(command.EndTime),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(Enumerable.Empty<GameWagerInfo>().ToAsyncEnumerable());

        var service = new PullRecordCommandService(
            fakeLogger,
            fakeClient,
            fakeService);

        await service.ExecuteAsync(command: command);
    }
}
