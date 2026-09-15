using IPMS.IdentityService.DTOs;

namespace IPMS.IdentityService.Services;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);

    Task<LoginResponse?> RegisterAsync(RegisterRequest request);
}