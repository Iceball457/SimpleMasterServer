using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SimpleMasterServer.Results;

namespace SimpleMasterServer.Registration {
    public class ServerRegistry : ControllerBase {
        public List<ServerRegistration> Registrations { get; } = new();

        public Result RegisterServer(ServerData server) {
            Registrations.Add(new(server));
            Console.WriteLine($"New Server Added! {server.Game}, {server.Hostname}");
            return Result.SUCCESS;
        }

        public Result RemoveServer(ServerData server) {
            int removed = Registrations.RemoveAll(reg => reg.Server == server);
            if (removed == 0) {
                return new(false, "No servers were removed.");
            }
            return Result.SUCCESS;
        }

        public Result RemoveServersWhere(Predicate<ServerData> predicate) {
            int removed = Registrations.RemoveAll(reg => predicate(reg.Server));
            if (removed == 0) {
                return new(false, "No servers were removed.");
            }
            return Result.SUCCESS;
        }
        public Result RemoveServersWhere(Predicate<ServerRegistration> predicate) {
            int removed = Registrations.RemoveAll(predicate);
            if (removed == 0) {
                return new(false, "No servers were removed.");
            }
            return Result.SUCCESS;
        }
    }
}