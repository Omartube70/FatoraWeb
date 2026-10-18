namespace Fatora.Models.Api;

// What GET api/admin/dashboard, api/admin/devices and api/admin/devices/logins return.
// A server that predates them answers 404 — the dashboard then falls back to the old endpoints.

public class NamedCountDto
{
    public string Name { get; set; } = string.Empty;
    public int Count { get; set; }
}

/// <summary>One copy of the app (Windows or phone) with its owner and when it was last used.</summary>
public class DeviceOverviewDto
{
    public int ActivationId { get; set; }
    public int LicenseId { get; set; }
    public string LicenseKey { get; set; } = string.Empty;
    public int ClientId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string BusinessName { get; set; } = string.Empty;
    public string DeviceId { get; set; } = string.Empty;
    public string MachineName { get; set; } = string.Empty;
    public string? DeviceName { get; set; }

    /// <summary>"Desktop" or "Mobile".</summary>
    public string Platform { get; set; } = "Desktop";
    public string OperatingSystem { get; set; } = string.Empty;
    public string ApplicationVersion { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime ActivatedAt { get; set; }
    public DateTime? LastSeenAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public string? LastLoginUser { get; set; }
    public int LoginCount { get; set; }
    public bool IsOnline { get; set; }

    public string DisplayClient => string.IsNullOrWhiteSpace(BusinessName) ? ClientName : BusinessName;
    public string DisplayDevice => string.IsNullOrWhiteSpace(DeviceName) ? MachineName : DeviceName!;

    /// <summary>The most recent sign of life: a sign-in or a license check.</summary>
    public DateTime? LastActivity =>
        LastSeenAt is { } s && LastLoginAt is { } l ? (s > l ? s : l) : LastSeenAt ?? LastLoginAt;
}

public class LoginEventDto
{
    public long LoginEventId { get; set; }
    public int ActivationId { get; set; }
    public int LicenseId { get; set; }
    public int ClientId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string BusinessName { get; set; } = string.Empty;
    public string DeviceId { get; set; } = string.Empty;
    public string? DeviceName { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Platform { get; set; } = "Desktop";
    public string ApplicationVersion { get; set; } = string.Empty;
    public string? IpAddress { get; set; }
    public DateTime CreatedAt { get; set; }

    public string DisplayClient => string.IsNullOrWhiteSpace(BusinessName) ? ClientName : BusinessName;
}

public class DailyLoginsDto
{
    public DateTime Date { get; set; }
    public int Desktop { get; set; }
    public int Mobile { get; set; }
}

public class ExpiringLicenseDto
{
    public int LicenseId { get; set; }
    public int ClientId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string BusinessName { get; set; } = string.Empty;
    public string PlanName { get; set; } = string.Empty;
    public string Platform { get; set; } = "Both";
    public DateTime ExpiryDate { get; set; }
    public int DaysLeft { get; set; }

    public string DisplayClient => string.IsNullOrWhiteSpace(BusinessName) ? ClientName : BusinessName;
}

public class RecentActivityDto
{
    public int AuditLogId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? ClientId { get; set; }
    public string? ClientName { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class DashboardClientsDto
{
    public int Total { get; set; }
    public int Active { get; set; }
    public int Blocked { get; set; }
    public int NewThisMonth { get; set; }
    public int NewLast30Days { get; set; }
    public int TrialsUsed { get; set; }
}

public class DashboardLicensesDto
{
    public int Total { get; set; }
    public int Active { get; set; }
    public int Pending { get; set; }
    public int Suspended { get; set; }
    public int Revoked { get; set; }
    public int Expired { get; set; }
    public int ExpiringIn7Days { get; set; }
    public int ExpiringIn30Days { get; set; }
    public int SyncEnabled { get; set; }
    public int IssuedThisMonth { get; set; }
    public List<NamedCountDto> ByPlan { get; set; } = new();
    public List<NamedCountDto> ByPlatform { get; set; } = new();
}

public class DashboardDevicesDto
{
    public int Total { get; set; }
    public int Active { get; set; }
    public int Inactive { get; set; }
    public int Revoked { get; set; }
    public int Desktop { get; set; }
    public int Mobile { get; set; }
    public int OnlineNow { get; set; }
    public int SeenLast24Hours { get; set; }
    public int Stale { get; set; }
    public int NeverReportedLogin { get; set; }
    public List<NamedCountDto> Versions { get; set; } = new();
}

public class DashboardLoginsDto
{
    public int Last24Hours { get; set; }
    public int Last7Days { get; set; }
    public int Last30Days { get; set; }
    public int ActiveUsersLast7Days { get; set; }
    public List<DailyLoginsDto> Daily { get; set; } = new();
}

public class DashboardSyncDto
{
    public bool Available { get; set; }
    public int Devices { get; set; }
    public int ActiveDevices { get; set; }
    public DateTime? LastActivityAt { get; set; }
}

public class DailyCountDto
{
    public DateTime Date { get; set; }
    public long Count { get; set; }
}

public class DailyTrafficDto
{
    public DateTime Date { get; set; }
    public long Total { get; set; }
    public long License { get; set; }
    public long Sync { get; set; }
    public long Admin { get; set; }
    public long Other { get; set; }
    public long Errors { get; set; }
}

public class HourlyTrafficDto
{
    public int Hour { get; set; }
    public long Total { get; set; }
}

public class DashboardTrafficDto
{
    public bool HasData { get; set; }
    public DateTime? TrackingSince { get; set; }
    public long RequestsToday { get; set; }
    public long RequestsYesterday { get; set; }
    public long RequestsThisMonth { get; set; }
    public long ErrorsToday { get; set; }
    public long ErrorsThisMonth { get; set; }
    public double AvgPerDayThisMonth { get; set; }
    public double AvgPerDayLast7Days { get; set; }
    public double AvgPerDayLast30Days { get; set; }
    public double AvgPerHourToday { get; set; }
    public long BusiestDayRequests { get; set; }
    public DateTime? BusiestDay { get; set; }
    public long BusiestHourRequests { get; set; }
    public DateTime? BusiestHour { get; set; }
    public List<NamedCountDto> TodayByKind { get; set; } = new();
    public List<NamedCountDto> MonthByKind { get; set; } = new();
    public List<DailyTrafficDto> Daily { get; set; } = new();
    public List<HourlyTrafficDto> HoursToday { get; set; } = new();
}

public class DashboardInvoicesDto
{
    public bool Available { get; set; }
    public long Today { get; set; }
    public long Yesterday { get; set; }
    public long ThisMonth { get; set; }
    public long Last30Days { get; set; }
    public long Total { get; set; }
    public double AvgPerDayThisMonth { get; set; }
    public double AvgPerDayLast30Days { get; set; }
    public long BusiestDayInvoices { get; set; }
    public DateTime? BusiestDay { get; set; }
    public int ShopsLast30Days { get; set; }
    public double AvgPerShopPerDay { get; set; }
    public List<DailyCountDto> Daily { get; set; } = new();
}

public class AdminDashboardDto
{
    public DateTime GeneratedAt { get; set; }
    public int TzOffsetMinutes { get; set; }
    public DashboardTrafficDto Traffic { get; set; } = new();
    public DashboardInvoicesDto Invoices { get; set; } = new();
    public DashboardClientsDto Clients { get; set; } = new();
    public DashboardLicensesDto Licenses { get; set; } = new();
    public DashboardDevicesDto Devices { get; set; } = new();
    public DashboardLoginsDto Logins { get; set; } = new();
    public DashboardSyncDto Sync { get; set; } = new();
    public List<DeviceOverviewDto> LastLogins { get; set; } = new();
    public List<LoginEventDto> RecentLogins { get; set; } = new();
    public List<ExpiringLicenseDto> ExpiringLicenses { get; set; } = new();
    public List<RecentActivityDto> RecentActivity { get; set; } = new();
}
