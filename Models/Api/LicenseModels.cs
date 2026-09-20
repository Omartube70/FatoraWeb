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

    /// <summary>"Both" | "Desktop" | "Mobile" — which app this key opens.</summary>
    public string Platform { get; set; } = "Both";

    /// <summary>Whether this shop's devices may link through the server (sync).</summary>
    public bool AllowSync { get; set; } = true;

    /// <summary>The shop's feature list as JSON — null = everything (see <see cref="LicenseFeatures"/>).</summary>
    public string? Features { get; set; }

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
    public string Platform { get; set; } = "Both";
    public bool AllowSync { get; set; } = true;
    public string? Features { get; set; }
}

public class UpdateLicenseRequest
{
    public int PlanId { get; set; }
    public int MaxDevices { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string Platform { get; set; } = "Both";
    public bool AllowSync { get; set; } = true;
    public string? Features { get; set; }
}
