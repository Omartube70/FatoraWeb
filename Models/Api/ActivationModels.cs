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
    public string Status { get; set; } = string.Empty;
    public DateTime? DeactivatedAt { get; set; }
}
