using System.Security.Claims;
using Fatora.Models.Api;
using Fatora.Services.Api;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Fatora.Services.Auth;

// Plain (non-Blazor) endpoints for the login/logout POST.
// Writing the auth cookie requires HttpContext.SignInAsync to run during a normal
// request/response cycle - that is not possible from inside an interactive Blazor
// Server circuit, so the login form posts here instead of using Blazor event handlers.
public static class AdminAuthEndpoints
{
    public static void MapAdminAuthEndpoints(this WebApplication app)
    {
        app.MapPost("/admin/account/login", HandleLoginAsync);
        app.MapPost("/admin/account/logout", HandleLogoutAsync);
    }

    private static async Task<IResult> HandleLoginAsync(
        HttpContext http,
        IAntiforgery antiforgery,
        AuthService authService)
    {
        try
        {
            await antiforgery.ValidateRequestAsync(http);
        }
        catch (AntiforgeryValidationException)
        {
            return Results.Redirect($"{AdminAuthConstants.LoginPath}?error=" + Uri.EscapeDataString("انتهت صلاحية الجلسة، من فضلك حاول مرة أخرى"));
        }

        var form = await http.Request.ReadFormAsync();
        var username = form["username"].ToString();
        var password = form["password"].ToString();
        var returnUrl = form["returnUrl"].ToString();

        try
        {
            var result = await authService.LoginAsync(username, password);

            var email = JwtHelper.TryGetClaim(result.AccessToken!, "email");
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, result.AdminUserId.ToString()),
                new(ClaimTypes.Name, result.Username ?? username),
                new(AdminAuthConstants.ApiTokenClaimType, result.AccessToken!)
            };
            if (!string.IsNullOrEmpty(email))
                claims.Add(new Claim("email", email));

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await http.SignInAsync(AdminAuthConstants.CookieScheme, principal, new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
            });

            var target = string.IsNullOrWhiteSpace(returnUrl) || !returnUrl.StartsWith('/') ? "/admin" : returnUrl;
            return Results.Redirect(target);
        }
        catch (ApiException ex)
        {
            return Results.Redirect($"{AdminAuthConstants.LoginPath}?error=" + Uri.EscapeDataString(ex.Message));
        }
    }

    private static async Task<IResult> HandleLogoutAsync(HttpContext http, IAntiforgery antiforgery)
    {
        try
        {
            await antiforgery.ValidateRequestAsync(http);
        }
        catch (AntiforgeryValidationException)
        {
            // still sign the user out even if the token expired - logging out is safe either way
        }

        await http.SignOutAsync(AdminAuthConstants.CookieScheme);
        return Results.Redirect(AdminAuthConstants.LoginPath);
    }
}
