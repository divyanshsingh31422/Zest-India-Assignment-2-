using Microsoft.Extensions.Configuration;
using Moq;
using StudentManagement.Api.Models;
using StudentManagement.Api.Services;
using Xunit;

namespace StudentManagement.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<ILogger<AuthService>> _mockLogger;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _mockConfiguration = new Mock<IConfiguration>();
        _mockLogger = new Mock<ILogger<AuthService>>();

        // Setup mock configuration
        var jwtSettings = new Dictionary<string, string>
        {
            {"JwtSettings:SecretKey", "ThisIsASecretKeyForJWTTokenGeneration123456789"},
            {"JwtSettings:Issuer", "StudentManagementAPI"},
            {"JwtSettings:Audience", "StudentManagementUsers"},
            {"JwtSettings:ExpirationInMinutes", "60"}
        };

        _mockConfiguration.Setup(c => c["JwtSettings:SecretKey"]).Returns(jwtSettings["JwtSettings:SecretKey"]);
        _mockConfiguration.Setup(c => c["JwtSettings:Issuer"]).Returns(jwtSettings["JwtSettings:Issuer"]);
        _mockConfiguration.Setup(c => c["JwtSettings:Audience"]).Returns(jwtSettings["JwtSettings:Audience"]);
        _mockConfiguration.Setup(c => c["JwtSettings:ExpirationInMinutes"]).Returns(jwtSettings["JwtSettings:ExpirationInMinutes"]);

        _authService = new AuthService(_mockConfiguration.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ShouldReturnToken()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Email = "admin@studentmanagement.com",
            Password = "Admin123!"
        };

        // Act
        var result = await _authService.LoginAsync(loginRequest);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Token);
        Assert.True(result.Expiration > DateTime.UtcNow);
        Assert.True(result.Expiration <= DateTime.UtcNow.AddMinutes(60));
    }

    [Fact]
    public async Task LoginAsync_WithInvalidCredentials_ShouldThrowException()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Email = "admin@studentmanagement.com",
            Password = "WrongPassword"
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _authService.LoginAsync(loginRequest));

        Assert.Equal("Invalid email or password", exception.Message);
    }

    [Fact]
    public async Task LoginAsync_WithNonExistentUser_ShouldThrowException()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Email = "nonexistent@example.com",
            Password = "Password123!"
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _authService.LoginAsync(loginRequest));

        Assert.Equal("Invalid email or password", exception.Message);
    }

    [Theory]
    [InlineData("admin@studentmanagement.com", "Admin123!", "Admin")]
    [InlineData("teacher@studentmanagement.com", "Teacher123!", "Teacher")]
    [InlineData("user@studentmanagement.com", "User123!", "User")]
    public async Task LoginAsync_WithDifferentValidUsers_ShouldReturnToken(string email, string password, string expectedRole)
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Email = email,
            Password = password
        };

        // Act
        var result = await _authService.LoginAsync(loginRequest);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Token);
        Assert.True(result.Expiration > DateTime.UtcNow);
    }
}
