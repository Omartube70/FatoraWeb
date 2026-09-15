using Microsoft.AspNetCore.Components.Authorization;

namespace Fatora.Services.Api;

public class ActivationsService : ApiServiceBase
{
    public ActivationsService(HttpClient http, AuthenticationStateProvider authStateProvider) : base(http, authStateProvider) { }

    public Task ReactivateAsync(int activationId, CancellationToken ct = default)
        => PostAsync($"api/admin/AdminLicenses/activations/{activationId}/reactivate", null, ct);

    public Task RevokeAsync(int activationId, CancellationToken ct = default)
        => PostAsync($"api/admin/AdminLicenses/activations/{activationId}/revoke", null, ct);
}
