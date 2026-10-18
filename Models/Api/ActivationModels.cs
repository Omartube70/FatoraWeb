namespace Fatora.Models.Api;

public class ActivationDto
{
    public int ActivationId { get; set; }
    public int LicenseId { get; set; }
    public string DeviceId { get; set; } = string.Empty;
    public string MachineName { get; set; } = string.Empty;
    public string? DeviceName { get; set; }
    public string OperatingSystem { get; set; } = string.Empty;
    public string ApplicationVersion { get; set; } = string.Empty;
    public DateTime ActivatedAt { get; set; }
    public DateTime? LastSeenAt { get; set; }

    // Filled by a license server that tracks sign-ins; an older one leaves them empty.
    public DateTime? LastLoginAt { get; set; }
    public string? LastLoginUser { get; set; }
    public int LoginCount { get; set; }

    /// <summary>"Desktop" or "Mobile".</summary>
    public string Platform { get; set; } = "Desktop";

    public string Status { get; set; } = string.Empty;
    public DateTime? DeactivatedAt { get; set; }
}
