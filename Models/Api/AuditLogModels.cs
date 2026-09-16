namespace Fatora.Models.Api;

public class AuditLogDto
{
    public int AuditLogId { get; set; }
    public string Action { get; set; } = string.Empty;
    public int? LicenseId { get; set; }
    public int? ClientId { get; set; }
    public string? DeviceId { get; set; }
    public int? AdminUserId { get; set; }
    public string? IPAddress { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}
