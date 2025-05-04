using legal_document_analyzer.Domain.Entities;
using legal_document_analyzer.Application.DTOs;
namespace legal_document_analyzer.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User> Register(UserRegisterDTO user, string password);
        Task<User> Login(string username, string password);
        Task<bool> UserExists(string username);
        Task<User> GetUserById(Guid id);
        Task<User> GetUserByUsername(string username);
        Task<User> UpdateUser(User user);
        string GetRefreshToken(string username);
        void SaveRefreshToken(string username, string refreshToken);
        void RemoveRefreshToken(string username, string refreshToken);
    }
}
