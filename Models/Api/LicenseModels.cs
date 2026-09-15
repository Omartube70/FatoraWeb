namespace Fatora.Models.Api;

public class LicenseDto
{
    public int LicenseId { get; set; }
    public int ClientId { get; set; }
    public string LicenseKey { get; set; } = string.Empty;
    public int PlanId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int MaxDevices { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? LastValidationAt { get; set; }
}

public class CreateLicenseRequest
{
    public int ClientId { get; set; }
    public int PlanId { get; set; }
    public DateTime? StartDate { get; set; }
}

public class UpdateLicenseRequest
{
    public int PlanId { get; set; }
    public int MaxDevices { get; set; }
    public DateTime ExpiryDate { get; set; }
}
