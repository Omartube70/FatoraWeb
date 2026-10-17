namespace Fatora.Components.Site;

/// <summary>Contact details and helpers shared by the public site's components.</summary>
public static class SiteInfo
{
    public const string WhatsAppNumber = "201080197811";
    public const string WhatsAppDisplay = "+20 108 019 7811";
    public const string SupportEmail = "support@fatora.com";
    public const string ApkPath = "downloads/FatoraMobile.apk";

    public static string WhatsApp(string message)
        => $"https://wa.me/{WhatsAppNumber}?text={Uri.EscapeDataString(message)}";
}

/// <summary>
/// Language handling for the public site. The choice lives in a cookie so the very first
/// (server-rendered) HTML already has the right language and direction - no flash on load.
/// </summary>
public static class SiteLanguage
{
    public const string CookieName = "fx-lang";
    public const string Arabic = "ar";
    public const string English = "en";

    public static string Normalize(string? value)
        => string.Equals(value, English, StringComparison.OrdinalIgnoreCase) ? English : Arabic;

    public static string FromRequest(HttpContext? context)
        => Normalize(context?.Request.Cookies[CookieName]);

    public static string DirectionOf(string lang) => lang == English ? "ltr" : "rtl";
}
