using JobHunterAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace JobHunterAPI.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ApplicationDbContext dbContext)
        {
            // Try to get the API key from the headers
            if (!context.Request.Headers.TryGetValue("X-Api-Key", out var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API Key was not provided.");
                return;
            }

            // Handle the case where the header contains multiple values or is empty
            string apiKey = extractedApiKey.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid API Key.");
                return;
            }

            // Validate the API key from the database
            var apiKeyRecord = await dbContext.ApiKeys.SingleOrDefaultAsync(x => x.Key == apiKey && !x.IsRevoked);

            if (apiKeyRecord == null || (apiKeyRecord.ExpiresAt.HasValue && apiKeyRecord.ExpiresAt.Value < DateTime.UtcNow))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized client.");
                return;
            }

            await _next(context);
        }
    }

}

