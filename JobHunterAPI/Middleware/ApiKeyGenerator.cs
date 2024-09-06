namespace JobHunterAPI.Middleware
{
    public static class ApiKeyGenerator
    {
        public static string GenerateApiKey()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("=", "").Replace("+", "").Replace("/", "");
        }
    }
}
