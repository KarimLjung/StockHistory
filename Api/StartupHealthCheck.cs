using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Api
{
    public class StartupHealthCheck : IHealthCheck
    {
        public bool StartupCompleted { get; set; }
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            if (StartupCompleted)
            {
                return Task.FromResult(HealthCheckResult.Healthy("The startup task has completed."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("That startup task is still running."));
        }
    }
}
