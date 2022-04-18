using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Api
{
    public class StartupHealthCheck : IHealthCheck
    {
        public StartupHealthCheck(ILogger<StartupHealthCheck> logger)
        {
            Logger = logger;
        }
        public bool StartupCompleted { get; set; }
        public ILogger<StartupHealthCheck> Logger { get; }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            Logger.LogWarning("Testing 123!!!");

            if (StartupCompleted)
            {
                return Task.FromResult(HealthCheckResult.Healthy("The startup task has completed."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("That startup task is still running."));
        }
    }
}
