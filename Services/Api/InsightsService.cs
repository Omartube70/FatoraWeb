using Fatora.Models.Api;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fatora.Services.Api;

/// <summary>The dashboard, the devices page and the sign-in history (api/admin/dashboard, /devices, /devices/logins).</summary>
public class InsightsService : ApiServiceBase
{
    public InsightsService(HttpClient http, AuthenticationStateProvider authStateProvider) : base(http, authStateProvider) { }

    /// <param name="tzOffsetMinutes">How far the admin's clock is ahead of UTC, so "today" is their today.</param>
    public Task<AdminDashboardDto> GetDashboardAsync(int tzOffsetMinutes, CancellationToken ct = default)
        => GetAsync<AdminDashboardDto>($"api/admin/dashboard?tzOffsetMinutes={tzOffsetMinutes}", ct);

    public Task<List<DeviceOverviewDto>> GetDevicesAsync(CancellationToken ct = default)
        => GetAsync<List<DeviceOverviewDto>>("api/admin/devices", ct);

    public Task<List<LoginEventDto>> GetLoginsAsync(int? activationId = null, int? licenseId = null, int take = 100, CancellationToken ct = default)
    {
        var query = $"take={take}";
        if (activationId.HasValue) query += $"&activationId={activationId}";
        if (licenseId.HasValue) query += $"&licenseId={licenseId}";
        return GetAsync<List<LoginEventDto>>($"api/admin/devices/logins?{query}", ct);
    }
}
