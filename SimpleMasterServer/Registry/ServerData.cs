namespace SimpleMasterServer.Registration {
    [Serializable]
    public record ServerData {
        public string Name { get; }
        public string Game { get; }
        public string Hostname { get; }
        public string AlivePath { get; }

        public ServerData(string name, string game, string hostname, string alivePath) {
            Name = name;
            Game = game;
            Hostname = hostname;
            AlivePath = alivePath;
        }
    }
}