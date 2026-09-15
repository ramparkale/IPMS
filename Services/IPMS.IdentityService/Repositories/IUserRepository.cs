using IPMS.IdentityService.Entities;

namespace IPMS.IdentityService.Repositories;

public interface IUserRepository
{
    Task<User?> GetByUserNameAsync(string userName);

    Task<bool> UserNameExistsAsync(string userName);

    Task<bool> EmailExistsAsync(string email);

    Task<User> CreateAsync(User user);

    Task<Role?> GetRoleAsync(string roleName);
}