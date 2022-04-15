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

        public void TestingSwitch()
        {
            var rect = new Shape()
            {
                Name = "Rectangle",
                Description = "testing 123"
            };


             var circle = new Shape()
            {
                Name = "Circle",
                Description = "testing 1234"
            };

            switch(rect)
            {
                case Shape when rect.Description == "testing123":
                    rect.Description = "this is a rectangle";
                    break;
            }

            string description = rect switch
            {
                Shape when rect.Description == "testing123" => "this is a rectangle",
                _ => throw new NotImplementedException()
            };




        }
    }
}
