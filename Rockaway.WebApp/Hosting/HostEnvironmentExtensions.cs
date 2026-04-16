// Rockaway.WebApp/Hosting/HostEnvironmentExtensions.cs

namespace Rockaway.WebApp.Hosting;

public static class HostEnvironmentExtensions {
	private static readonly string[] sqliteEnvironments = ["UnitTest", Environments.Development];

	extension(IHostEnvironment env) {
		public bool ShouldUseSqlite
			=> sqliteEnvironments.Contains(env.EnvironmentName);

		public bool ShouldDisplayDetailedErrors
			=> env.EnvironmentName is "Development" or "Staging";
	}
}