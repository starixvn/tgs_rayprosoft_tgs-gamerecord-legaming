using Tgs.GameRecord.LEGaming.Core.JobApplicationServices;

namespace Tgs.GameRecord.LEGaming.Core;

public interface IHangfireUtensil
{
	public Task JobCheckAsync();

	public Task JobExecuteAsync(JobExecuteCommand source);

	public Task JobInitAsync();
}
