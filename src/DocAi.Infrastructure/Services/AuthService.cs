using DocAi.Core.Entities;
using DocAi.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DocAi.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly string _secretKey;

    public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _secretKey = configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key");
    }

    public async Task<string> RegisterAsync(string fullName, string email, string password)
    {
        if (await _unitOfWork.Repository<User>().AnyAsync(x => x.Email == email))
            throw new Exception("Email already exists");

        var user = new User
        {
            FullName = fullName,
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Role = "User"
        };

        await _unitOfWork.Repository<User>().AddAsync(user);
        await _unitOfWork.CommitAsync();
        return "User registered successfully";
    }

    public async Task<string> LoginAsync(string email, string password)
    {
        var users = await _unitOfWork.Repository<User>().Where(x => x.Email == email);
        var user = users.FirstOrDefault();
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            throw new Exception("Invalid credentials");

        return GenerateToken(user);
    }

    public async Task ForgotPasswordAsync(string email)
    {
        var users = await _unitOfWork.Repository<User>().Where(x => x.Email == email);
        if (!users.Any()) throw new Exception("User not found");
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var users = await _unitOfWork.Repository<User>().Where(x => x.Email == email);
        return users.FirstOrDefault();
    }

    private string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
