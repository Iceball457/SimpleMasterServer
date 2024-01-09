namespace SimpleMasterServer.Results {
    public record Result {
        public bool Success { get; }
        public string Message { get; }

        public static readonly Result SUCCESS = new(true, "");
        public static readonly Result FAIL = new(false, "");

        public Result(bool success, string message) {
            Success = success;
            Message = message;
        }
    }
}