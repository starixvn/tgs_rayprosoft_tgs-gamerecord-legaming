using Tgs.GameRecord.LEGaming.Core.ApplicationServices;

namespace Tgs.GameRecord.LEGaming.Core;

public interface IPullRecordDispatcher
{
	ValueTask PushCommandAsync(PullRecordCommand command, CancellationToken cancellationToken = default);
}
