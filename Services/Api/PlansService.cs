using Fatora.Models.Api;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fatora.Services.Api;

public class PlansService : ApiServiceBase
{
    public PlansService(HttpClient http, AuthenticationStateProvider authStateProvider) : base(http, authStateProvider) { }

    public Task<List<PlanDto>> GetAllAsync(CancellationToken ct = default)
        => GetAsync<List<PlanDto>>("api/admin/Plans", ct);

    public Task<PlanDto> GetByIdAsync(int id, CancellationToken ct = default)
        => GetAsync<PlanDto>($"api/admin/Plans/{id}", ct);

    public Task<PlanDto> CreateAsync(CreatePlanRequest request, CancellationToken ct = default)
        => PostAsync<PlanDto>("api/admin/Plans", request, ct);

    public Task<PlanDto> UpdateAsync(int id, UpdatePlanRequest request, CancellationToken ct = default)
        => PutAsync<PlanDto>($"api/admin/Plans/{id}", request, ct);

    public Task DeactivateAsync(int id, CancellationToken ct = default)
        => PostAsync($"api/admin/Plans/{id}/deactivate", null, ct);

    public Task ActivateAsync(int id, CancellationToken ct = default)
        => PostAsync($"api/admin/Plans/{id}/activate", null, ct);
}
