using IPMS.IdentityService.Data;
using IPMS.IdentityService.Entities;
using Microsoft.EntityFrameworkCore;

namespace IPMS.IdentityService.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IdentityDbContext _context;

    public UserRepository(IdentityDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByUserNameAsync(
        string userName)
    {
        return await _context.Users
            .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                    .ThenInclude(x => x.RolePermissions)
                        .ThenInclude(x => x.Permission)
            .FirstOrDefaultAsync(
                x => x.UserName == userName &&
                     x.IsActive);
    }

    public async Task<bool> UserNameExistsAsync(
        string userName)
    {
        return await _context.Users
            .AnyAsync(x => x.UserName == userName);
    }

    public async Task<bool> EmailExistsAsync(
        string email)
    {
        return await _context.Users
            .AnyAsync(x => x.Email == email);
    }

    public async Task<User> CreateAsync(User user)
    {
        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<Role?> GetRoleAsync(
        string roleName)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(
                x => x.RoleName == roleName &&
                     x.IsActive);
    }
}