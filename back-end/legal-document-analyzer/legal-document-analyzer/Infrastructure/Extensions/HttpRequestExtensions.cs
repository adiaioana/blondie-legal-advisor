using Microsoft.AspNetCore.Http;

namespace legal_document_analyzer.Infrastructure.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string GetBearerToken(this HttpRequest request)
        {
            var authHeader = request.Headers["Authorization"].ToString();
            return authHeader.StartsWith("Bearer ") ? authHeader.Substring("Bearer ".Length).Trim() : null;
        }
    }
}
