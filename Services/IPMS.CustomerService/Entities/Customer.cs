namespace IPMS.CustomerService.Entities;

public class Customer
{
    public int CustomerId { get; set; }

    public string CustomerCode { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string? LastName { get; set; }

    public DateTime? DOB { get; set; }

    public string? Gender { get; set; }

    public string Email { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Pincode { get; set; }

    public string KycStatus { get; set; } = "Pending";

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }
}