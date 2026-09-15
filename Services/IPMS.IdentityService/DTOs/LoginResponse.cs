namespace IPMS.IdentityService.DTOs;

public class LoginResponse
{
    public int UserId { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Token { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }

    public List<string> Roles { get; set; } = new();

    public List<string> Permissions { get; set; } = new();
}