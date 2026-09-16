using Fatora.Models.Api;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fatora.Services.Api;

public class AuditLogsService : ApiServiceBase
{
    public AuditLogsService(HttpClient http, AuthenticationStateProvider authStateProvider) : base(http, authStateProvider) { }

    public Task<List<AuditLogDto>> GetRecentAsync(int count = 200, CancellationToken ct = default)
        => GetAsync<List<AuditLogDto>>($"api/admin/AuditLogs?count={count}", ct);
}
