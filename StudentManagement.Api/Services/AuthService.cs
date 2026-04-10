using Microsoft.IdentityModel.Tokens;
using StudentManagement.Api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentManagement.Api.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IConfiguration configuration, ILogger<AuthService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
    {
        try
        {
            _logger.LogInformation("Attempting login for email: {Email}", loginRequest.Email);

            // For demo purposes, we'll use a simple hardcoded validation
            // In a real application, you would validate against a database
            if (IsValidUser(loginRequest.Email, loginRequest.Password))
            {
                var token = GenerateJwtToken(loginRequest.Email);
                var expiration = DateTime.UtcNow.AddMinutes(
                    _configuration.GetValue<int>("JwtSettings:ExpirationInMinutes", 60));

                _logger.LogInformation("Login successful for email: {Email}", loginRequest.Email);

                return new LoginResponse
                {
                    Token = token,
                    Expiration = expiration
                };
            }

            _logger.LogWarning("Login failed for email: {Email}", loginRequest.Email);
            throw new UnauthorizedAccessException("Invalid email or password");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during login for email: {Email}", loginRequest.Email);
            throw;
        }
    }

    private bool IsValidUser(string email, string password)
    {
        // Demo users - in a real application, this would be validated against a database
        var demoUsers = new Dictionary<string, string>
        {
            { "admin@studentmanagement.com", "Admin123!" },
            { "user@studentmanagement.com", "User123!" },
            { "teacher@studentmanagement.com", "Teacher123!" }
        };

        return demoUsers.TryGetValue(email.ToLower(), out var storedPassword) && storedPassword == password;
    }

    private string GenerateJwtToken(string email)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");
        var issuer = jwtSettings["Issuer"] ?? "StudentManagementAPI";
        var audience = jwtSettings["Audience"] ?? "StudentManagementUsers";

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, email),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, email),
            new Claim(ClaimTypes.Role, GetUserRole(email))
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(jwtSettings.GetValue<int>("ExpirationInMinutes", 60)),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GetUserRole(string email)
    {
        // Demo role assignment - in a real application, this would come from the database
        if (email.Equals("admin@studentmanagement.com", StringComparison.OrdinalIgnoreCase))
            return "Admin";
        
        if (email.Equals("teacher@studentmanagement.com", StringComparison.OrdinalIgnoreCase))
            return "Teacher";
        
        return "User";
    }
}
