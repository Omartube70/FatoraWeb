using Microsoft.AspNetCore.Components.Authorization;

namespace Fatora.Services.Auth;

public class CurrentAdminService
{
    private readonly AuthenticationStateProvider _authStateProvider;

    public CurrentAdminService(AuthenticationStateProvider authStateProvider)
    {
        _authStateProvider = authStateProvider;
    }

    public async Task<string?> GetUsernameAsync()
    {
        var state = await _authStateProvider.GetAuthenticationStateAsync();
        return state.User.Identity?.Name;
    }

    public async Task<string?> GetEmailAsync()
    {
        var state = await _authStateProvider.GetAuthenticationStateAsync();
        return state.User.FindFirst("email")?.Value;
    }

    public async Task<string?> GetAdminUserIdAsync()
    {
        var state = await _authStateProvider.GetAuthenticationStateAsync();
        return state.User.FindFirst("sub")?.Value;
    }
}
