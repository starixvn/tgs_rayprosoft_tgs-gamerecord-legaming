using Tgs.GameRecord.LEGaming.Core.JobApplicationServices;

namespace Tgs.GameRecord.LEGaming.Core;

public interface IHangfireUtensilRepository
{
	public Task JobCheckAsync();

	public Task JobExecuteAsync(JobExecuteCommand source);

	public Task JobInitAsync();
}
