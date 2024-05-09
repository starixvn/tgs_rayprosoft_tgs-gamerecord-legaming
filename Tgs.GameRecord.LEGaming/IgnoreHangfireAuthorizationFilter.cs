using Hangfire.Dashboard;

namespace Tgs.GameRecord.LEGaming;

public class IgnoreHangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
	public bool Authorize(DashboardContext context) => true;
}
