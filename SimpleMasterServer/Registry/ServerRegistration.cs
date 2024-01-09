namespace SimpleMasterServer.Registration {
    public class ServerRegistration {
        public ServerData Server { get; }
        public DateTime CheckIn { get; set; }

        public bool MostRecentResult { get; set; }

        public ServerRegistration(ServerData server) {
            Server = server;
            CheckIn = DateTime.UtcNow;
            MostRecentResult = true;
        }
    }
}