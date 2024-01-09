using SimpleMasterServer.Registration;
using SimpleMasterServer.Results;

namespace SimpleMasterServer.Checkup {
    public class CheckupService : BackgroundService {
        private readonly ServerRegistry registry;
        private readonly TimeSpan interval;
        private readonly TimeSpan lifespan;
        HttpClient networkClient = new();

        public CheckupService(ServerRegistry registry, int lifespan, int interval) {
            this.registry = registry;
            this.interval = TimeSpan.FromSeconds(interval);
            this.lifespan = TimeSpan.FromSeconds(lifespan);
        }

        /// <summary>
        /// Removes servers from the provided list, given that they haven't responded to alive checks in time.
        /// </summary>
        /// <param name="threshold">The oldest timestamp allowed on servers that fail to respond.</param>
        /// <returns>A result with the number of servers purged in the message.</returns>
        public async Task<Result> CheckIn(DateTime threshold) {
            foreach (ServerRegistration registration in registry.Registrations) {
                // Check in with the server at the alive path, using an API call
                Result pingResult = await CallAliveEndpoint(registration.Server.AlivePath);
                // Determine if the server is running based on its response
                if (pingResult.Success == true) {
                    // If the server did respond, update its age
                    registration.CheckIn = DateTime.UtcNow;
                    registration.MostRecentResult = true;
                    //return Result.SUCCESS;
                }
                registration.MostRecentResult = false;
                //return Result.FAIL;
            }
            Console.WriteLine(registry.Registrations.RemoveAll(reg => reg.CheckIn < threshold));
            return Result.SUCCESS;
        }
        public async Task<Result> CallAliveEndpoint(string alivePath) {
            try {
                HttpResponseMessage response = await networkClient.GetAsync(alivePath);
                if (response.IsSuccessStatusCode) {
                    return Result.SUCCESS;
                } else {
                    return Result.FAIL;
                }
            } catch {
                return Result.FAIL;
            }

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            while (!stoppingToken.IsCancellationRequested) {
                await CheckIn(DateTime.UtcNow - lifespan);
                Console.WriteLine($"Checkup Service Executed at {DateTime.UtcNow}");
                await Task.Delay(interval, stoppingToken);
            }
        }
    }
}