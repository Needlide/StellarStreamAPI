using StellarStreamAPI.Interfaces;

namespace StellarStreamAPI.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMongoSecurityDatabaseContext _dbContext;
        private readonly IEncryptor _encryptor;

        public ApiKeyMiddleware(RequestDelegate next, IMongoSecurityDatabaseContext dbContext, IEncryptor encryptor)
        {
            _next = next;
            _dbContext = dbContext;
            _encryptor = encryptor;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Path.StartsWithSegments("/api/Administrative"))
            {
                var apiKey = context.Request.Headers["X-API-KEY"].FirstOrDefault();

                if (string.IsNullOrEmpty(apiKey))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsJsonAsync(new { error = "API key is required." });
                    return;
                }

                var result = await _dbContext.GetApiKeysAsync();
                var storedKey = result.Value.Find(k => _encryptor.Decrypt(k.KeyValue) == apiKey);

                if (storedKey == null)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsJsonAsync(new { error = "Invalid API key." });
                    return;
                }

                if (storedKey.IsExpired)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsJsonAsync(new { error = "API key has expired." });

                    await _dbContext.DeleteApiKeyAsync(storedKey.KeyId);

                    return;
                }

                var now = DateTime.UtcNow;
                var nextAllowedUse = storedKey.LastUsed.Add(new TimeSpan(1, 0, 0));

                if (storedKey.RequestsThisHour > 59)
                {
                    var waitTime = (nextAllowedUse - now).TotalSeconds;

                    context.Response.Headers.Add("Retry-After", waitTime.ToString());

                    context.Response.StatusCode = 429;
                    await context.Response.WriteAsJsonAsync(new { error = $"Reached the limit of usage per hour. Try again in {waitTime} seconds.", limit = 60 });
                }

                storedKey.RequestsThisHour++;
                storedKey.LastUsed = now;

                _dbContext.UpdateApiKey(apiKey, storedKey);
            }
            await _next(context);
        }
    }
}
