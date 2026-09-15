using Fatora.Models.Api;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fatora.Services.Api;

public class EmailService : ApiServiceBase
{
    public EmailService(HttpClient http, AuthenticationStateProvider authStateProvider) : base(http, authStateProvider) { }

    public Task SendShiftSummaryAsync(ShiftSummaryRequest request, CancellationToken ct = default)
        => PostAsync("api/Email/send-shift-summary", request, ct);
}
