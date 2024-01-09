namespace SimpleMasterServer.Registration {
    public record ServerData {
        public string Game { get; }
        public string Mode { get; }
        public string Hostname { get; }
        public string AlivePath { get; }

        public ServerData(string game, string mode, string hostname, string alivePath) {
            Game = game;
            Mode = mode;
            Hostname = hostname;
            AlivePath = alivePath;
        }
    }
}