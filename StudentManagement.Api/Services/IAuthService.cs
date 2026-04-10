using StudentManagement.Api.Models;

namespace StudentManagement.Api.Services;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
}
