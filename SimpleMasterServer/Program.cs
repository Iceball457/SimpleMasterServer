using Microsoft.AspNetCore.Mvc;
using SimpleMasterServer.Checkup;
using SimpleMasterServer.Registration;

namespace SimpleMasterServer {
    internal class Program {
        private static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton<ServerRegistry>();
            builder.Services.AddHostedService<CheckupService>(provider => {
                return new(provider.GetRequiredService<ServerRegistry>(), 3, 2);
            });

            var app = builder.Build();

            app.MapGet("/", () => {
                Console.WriteLine("Hello World!");
                return "Hello World!\n";
            });

            app.MapPost("/register/", (ServerData data, [FromServices] ServerRegistry registry) => {
                registry.RegisterServer(data);
                return $"Registered {data.Game}\n";
            });


            app.Run();
        }
    }
}