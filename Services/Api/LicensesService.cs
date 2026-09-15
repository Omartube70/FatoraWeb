using Fatora.Models.Api;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fatora.Services.Api;

public class LicensesService : ApiServiceBase
{
    public LicensesService(HttpClient http, AuthenticationStateProvider authStateProvider) : base(http, authStateProvider) { }

    public Task<List<LicenseDto>> GetAllAsync(CancellationToken ct = default)
        => GetAsync<List<LicenseDto>>("api/admin/AdminLicenses", ct);

    public Task<LicenseDto> GetByIdAsync(int id, CancellationToken ct = default)
        => GetAsync<LicenseDto>($"api/admin/AdminLicenses/{id}", ct);

    public Task<LicenseDto> CreateAsync(CreateLicenseRequest request, CancellationToken ct = default)
        => PostAsync<LicenseDto>("api/admin/AdminLicenses", request, ct);

    public Task<LicenseDto> UpdateAsync(int id, UpdateLicenseRequest request, CancellationToken ct = default)
        => PutAsync<LicenseDto>($"api/admin/AdminLicenses/{id}", request, ct);

    public Task SuspendAsync(int id, CancellationToken ct = default)
        => PostAsync($"api/admin/AdminLicenses/{id}/suspend", null, ct);

    public Task RevokeAsync(int id, CancellationToken ct = default)
        => PostAsync($"api/admin/AdminLicenses/{id}/revoke", null, ct);

    // The backend binds newPlanId from the query string ([FromQuery]), not a JSON body.
    public Task RenewAsync(int id, int? newPlanId = null, CancellationToken ct = default)
    {
        var url = $"api/admin/AdminLicenses/{id}/renew";
        if (newPlanId.HasValue)
            url += $"?newPlanId={newPlanId.Value}";
        return PostAsync(url, null, ct);
    }

    public Task<List<ActivationDto>> GetActivationsAsync(int licenseId, CancellationToken ct = default)
        => GetAsync<List<ActivationDto>>($"api/admin/AdminLicenses/{licenseId}/activations", ct);
}
