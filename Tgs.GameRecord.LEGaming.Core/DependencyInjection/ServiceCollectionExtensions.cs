using System.Reflection;
using Tgs.GameCommand.Client;
using Tgs.GameInterface.LEGaming.Client;
using Tgs.GameRecord.LEGaming.Core;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddGameRecordCore(
	   this IServiceCollection services,
	   Action<GameCommandClientOptions, IServiceProvider> gameCommandClientOptions,
	   Action<GameInterfaceLEGamingClientOptions, IServiceProvider> gameInterfaceLEGamingClientOptions)
		=> services
		.RegisterAllServiceUtilityTypes(
			Assembly.GetExecutingAssembly(),
			ServiceLifetime.Transient)
		.AddGameCommandClient(gameCommandClientOptions)
		.AddGameInterfaceLEGamingClient(gameInterfaceLEGamingClientOptions)
		.AddSingleton<IPullRecordDispatcher, PullRecordDispatcher>()
		.AddTransient<IHangfireUtensil, HangfireUtensil>();
}
