namespace Fatora.Services.Auth;

public static class AdminAuthConstants
{
    public const string CookieScheme = "FatoraAdminCookie";
    public const string LoginPath = "/admin/login";
    public const string AccessDeniedPath = "/admin/access-denied";
    public const string ApiTokenClaimType = "fatora_api_token";
}
