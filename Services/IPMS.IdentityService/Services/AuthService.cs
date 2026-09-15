using IPMS.IdentityService.DTOs;
using IPMS.IdentityService.Entities;
using IPMS.IdentityService.Repositories;
using IPMS.IdentityService.Security;

namespace IPMS.IdentityService.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _repository;
    private readonly JwtTokenService _jwtTokenService;

    public AuthService(
        IUserRepository repository,
        JwtTokenService jwtTokenService)
    {
        _repository = repository;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<LoginResponse?> LoginAsync(
        LoginRequest request)
    {
        var user =
            await _repository.GetByUserNameAsync(
                request.UserName);

        if (user == null)
            return null;

        var validPassword =
            BCrypt.Net.BCrypt.Verify(
                request.Password,
                user.PasswordHash);

        if (!validPassword)
            return null;

        var roles = user.UserRoles
            .Where(x => x.Role.IsActive)
            .Select(x => x.Role.RoleName)
            .Distinct()
            .ToList();

        var permissions = user.UserRoles
            .Where(x => x.Role.IsActive)
            .SelectMany(x => x.Role.RolePermissions)
            .Where(x => x.Permission.IsActive)
            .Select(x => x.Permission.PermissionCode)
            .Distinct()
            .ToList();

        var jwt =
            _jwtTokenService.GenerateToken(
                user,
                roles,
                permissions);

        return new LoginResponse
        {
            UserId = user.UserId,
            UserName = user.UserName,
            Email = user.Email,
            Token = jwt.Token,
            ExpiresAt = jwt.ExpiresAt,
            Roles = roles,
            Permissions = permissions
        };
    }

    public async Task<LoginResponse?> RegisterAsync(
        RegisterRequest request)
    {
        if (await _repository.UserNameExistsAsync(
                request.UserName))
        {
            throw new Exception(
                "Username already exists.");
        }

        if (await _repository.EmailExistsAsync(
                request.Email))
        {
            throw new Exception(
                "Email already exists.");
        }

        var role =
            await _repository.GetRoleAsync(
                request.RoleName);

        if (role == null)
        {
            throw new Exception(
                "Invalid role.");
        }

        var user = new User
        {
            UserName = request.UserName,
            Email = request.Email,

            PasswordHash =
                BCrypt.Net.BCrypt.HashPassword(
                    request.Password),

            IsActive = true,

            CreatedDate = DateTime.UtcNow
        };

        user.UserRoles.Add(
            new UserRole
            {
                RoleId = role.RoleId,
                AssignedDate = DateTime.UtcNow
            });

        await _repository.CreateAsync(user);

        return new LoginResponse
        {
            UserId = user.UserId,
            UserName = user.UserName,
            Email = user.Email,
            Roles = new List<string>
            {
                role.RoleName
            }
        };
    }
}