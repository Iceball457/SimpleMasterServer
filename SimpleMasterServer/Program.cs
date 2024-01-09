using Microsoft.AspNetCore.Mvc;
using SimpleMasterServer.Checkup;
using SimpleMasterServer.Registration;

namespace SimpleMasterServer {
    internal class Program {
        private static void Main(string[] args) {

            if (args.Length != 2) {
                Console.WriteLine("Usage: program <server-lifespan> <check-in-frequency>");
                return;
            }
            int lifespan, interval;
            try {
                lifespan = int.Parse(args[0]);
                interval = int.Parse(args[1]);
            } catch {
                Console.WriteLine("Usage: program <server-lifespan> <check-in-frequency>\nBoth arguments must be integers.");
                return;
            }
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton<ServerRegistry>();
            builder.Services.AddHostedService<CheckupService>(provider => {
                return new(provider.GetRequiredService<ServerRegistry>(), lifespan, interval);
            });

            var app = builder.Build();

            app.MapGet("/", () => {
                Console.WriteLine("Hello World!");
                return "Hello World!\n";
            });

            app.MapPost("/register/", (ServerData data, [FromServices] ServerRegistry registry) => {
                registry.RegisterServer(data);
                return $"Registered {data.Name}\n";
            });

            app.MapGet("/getservers/", ([FromServices] ServerRegistry registry) => {
                return registry.GetServers();
            });

            app.Run();
        }
    }
}