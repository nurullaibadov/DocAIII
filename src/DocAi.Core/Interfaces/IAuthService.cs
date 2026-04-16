using DocAi.Core.Entities;

namespace DocAi.Core.Interfaces;

public interface IAuthService
{
    Task<string> RegisterAsync(string fullName, string email, string password);
    Task<string> LoginAsync(string email, string password);
    Task ForgotPasswordAsync(string email);
    Task<User?> GetUserByEmailAsync(string email);
}
