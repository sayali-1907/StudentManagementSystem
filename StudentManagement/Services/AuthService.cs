using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using StudentManagement.DTOs;
using StudentManagement.Models;
using StudentManagement.Repositories;

namespace StudentManagement.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IUserRepository userRepository, IConfiguration configuration, ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
    {
        _logger.LogInformation("Login attempt for username: {Username}", dto.Username);

        var user = await _userRepository.GetByUsernameAsync(dto.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            _logger.LogWarning("Failed login attempt for: {Username}", dto.Username);
            return null;
        }

        return GenerateAuthResponse(user);
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        _logger.LogInformation("Registering new user: {Username}", dto.Username);

        if (await _userRepository.UsernameExistsAsync(dto.Username))
            throw new InvalidOperationException($"Username '{dto.Username}' already exists.");

        var user = new User
        {
            Username = dto.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = dto.Role
        };

        var created = await _userRepository.CreateAsync(user);
        _logger.LogInformation("User registered: {Username}", created.Username);
        return GenerateAuthResponse(created);
    }

    private AuthResponseDto GenerateAuthResponse(User user)
    {
        var expiry = DateTime.UtcNow.AddHours(
            double.Parse(_configuration["Jwt:ExpiryHours"] ?? "24"));

        var token = GenerateJwtToken(user, expiry);

        return new AuthResponseDto
        {
            Token = token,
            Username = user.Username,
            Role = user.Role,
            Expiry = expiry
        };
    }

    private string GenerateJwtToken(User user, DateTime expiry)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expiry,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
