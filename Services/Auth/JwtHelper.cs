using System.Text.Json;

namespace Fatora.Services.Auth;

// Minimal, dependency-free reader for the JWT payload the API issues.
// We only ever need to *read* claims already trusted (the token just came back
// from our own login call over HTTPS) - no signature validation is performed here.
public static class JwtHelper
{
    public static string? TryGetClaim(string jwt, string claimName)
    {
        var payload = TryGetPayload(jwt);
        if (payload is null)
            return null;

        return payload.Value.TryGetProperty(claimName, out var value) ? value.GetString() : null;
    }

    private static JsonElement? TryGetPayload(string jwt)
    {
        try
        {
            var parts = jwt.Split('.');
            if (parts.Length < 2)
                return null;

            var payloadJson = Base64UrlDecode(parts[1]);
            return JsonSerializer.Deserialize<JsonElement>(payloadJson);
        }
        catch
        {
            return null;
        }
    }

    private static string Base64UrlDecode(string input)
    {
        var padded = input.Replace('-', '+').Replace('_', '/');
        switch (padded.Length % 4)
        {
            case 2: padded += "=="; break;
            case 3: padded += "="; break;
        }
        var bytes = Convert.FromBase64String(padded);
        return System.Text.Encoding.UTF8.GetString(bytes);
    }
}
