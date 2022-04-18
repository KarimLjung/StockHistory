namespace Api
{
    public class StartupBackgroundService : BackgroundService
    {
        private readonly StartupHealthCheck startupHealthCheck;

        public StartupBackgroundService(StartupHealthCheck startupHealthCheck)
        {
            this.startupHealthCheck = startupHealthCheck;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Sleeping for 15 seconds");
            await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
            startupHealthCheck.StartupCompleted = true;
        }

        public class Shape
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }
    }
}
