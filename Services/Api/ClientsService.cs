using Fatora.Models.Api;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fatora.Services.Api;

public class ClientsService : ApiServiceBase
{
    public ClientsService(HttpClient http, AuthenticationStateProvider authStateProvider) : base(http, authStateProvider) { }

    public Task<List<ClientDto>> GetAllAsync(CancellationToken ct = default)
        => GetAsync<List<ClientDto>>("api/admin/Clients", ct);

    public Task<ClientDto> GetByIdAsync(int id, CancellationToken ct = default)
        => GetAsync<ClientDto>($"api/admin/Clients/{id}", ct);

    public Task<ClientDto> CreateAsync(CreateClientRequest request, CancellationToken ct = default)
        => PostAsync<ClientDto>("api/admin/Clients", request, ct);

    public Task<ClientDto> UpdateAsync(int id, UpdateClientRequest request, CancellationToken ct = default)
        => PutAsync<ClientDto>($"api/admin/Clients/{id}", request, ct);

    public Task BlockAsync(int id, CancellationToken ct = default)
        => PostAsync($"api/admin/Clients/{id}/block", null, ct);

    public Task UnblockAsync(int id, CancellationToken ct = default)
        => PostAsync($"api/admin/Clients/{id}/unblock", null, ct);
}
