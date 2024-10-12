using backend.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class BaseUrlService : IBaseUrlService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<BaseUrlService> _logger;

    public BaseUrlService(IHttpContextAccessor httpContextAccessor, ILogger<BaseUrlService> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public virtual string GetBaseUrl() // Make it virtual
    {
        var request = _httpContextAccessor.HttpContext?.Request;

        if (request == null)
        {
            _logger.LogWarning("No active HTTP request context found.");
            return string.Empty;
        }

        var baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";
        _logger.LogInformation("Base URL generated: {BaseUrl}", baseUrl);

        return baseUrl;
    }
}
