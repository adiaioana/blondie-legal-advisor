
using System.Security.Claims;
using legal_document_analyzer.Domain.Entities;
namespace legal_document_analyzer.Infrastructure.Services
{
    public interface ITokenService
    {
        string GenerateJwtToken(User user);
        string GenerateJwtToken(ClaimsPrincipal principal);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        string GenerateRefreshToken();
    }
}
