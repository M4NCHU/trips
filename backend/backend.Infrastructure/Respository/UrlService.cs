using backend.Domain.Mappings;
using Microsoft.AspNetCore.Http;


namespace backend.Infrastructure.Services
{
    public class BaseUrlService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BaseUrlService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
           
            
        }

        public string GetBaseUrl()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            return $"{request.Scheme}://{request.Host}{request.PathBase}";
        }
    }
}

