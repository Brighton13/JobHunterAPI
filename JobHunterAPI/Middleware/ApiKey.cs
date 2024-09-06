namespace JobHunterAPI.Middleware
{
    public class ApiKey
    {
        public int Id { get; set; }
        public string Key { get; set; } = ApiKeyGenerator.GenerateApiKey();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }
    }
}
