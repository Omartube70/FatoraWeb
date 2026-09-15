using System.Net.Http.Json;
using Fatora.Models.Api;

namespace Fatora.Services.Api;

// Note: unlike every other admin endpoint, POST /api/Auth/login does NOT wrap its
// response in the {success,message,data,errors} envelope - it returns AdminLoginResponseDto
// flat on success (200) and the flat ApiResponse {success,message,errors} on failure (401/400).
// So this service talks to HttpClient directly instead of going through ApiServiceBase's
// envelope-unwrapping helpers.
public class AuthService
{
    private readonly HttpClient _http;

    public AuthService(HttpClient http)
    {
        _http = http;
    }

    public async Task<LoginResponse> LoginAsync(string username, string password, CancellationToken ct = default)
    {
        HttpResponseMessage response;
        try
        {
            response = await _http.PostAsJsonAsync("api/Auth/login", new LoginRequest { Username = username, Password = password }, ct);
        }
        catch (HttpRequestException ex)
        {
            throw new ApiException($"تعذر الاتصال بالسيرفر: {ex.Message}");
        }

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken: ct)
                     ?? new LoginResponse { Success = false, Message = "استجابة غير متوقعة من السيرفر" };

        if (!response.IsSuccessStatusCode || !result.Success || string.IsNullOrEmpty(result.AccessToken))
        {
            throw new ApiException(result.Message ?? "فشل تسجيل الدخول", (int)response.StatusCode);
        }

        return result;
    }
}
