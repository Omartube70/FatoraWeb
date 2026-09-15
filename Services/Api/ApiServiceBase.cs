using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Fatora.Models.Api;
using Fatora.Services.Auth;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fatora.Services.Api;

// The Bearer token is attached per-request here (rather than via a DelegatingHandler)
// because typed HttpClients build their handler pipeline through IHttpClientFactory's
// own internal (pooled, periodically-recreated) scope, which is NOT the same DI scope
// as the current Blazor Server circuit. A handler that captures AuthenticationStateProvider
// from that pooled scope throws ("GetAuthenticationStateAsync outside of the DI scope for
// a Razor component") because it isn't tied to the circuit. Reading the claim here works
// because ApiServiceBase's subclasses are resolved directly into the circuit's own scope.
public abstract class ApiServiceBase
{
    protected static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    protected readonly HttpClient Http;
    private readonly AuthenticationStateProvider? _authStateProvider;

    protected ApiServiceBase(HttpClient http, AuthenticationStateProvider? authStateProvider = null)
    {
        Http = http;
        _authStateProvider = authStateProvider;
    }

    private async Task<HttpRequestMessage> BuildRequestAsync(HttpMethod method, string url, object? body)
    {
        var request = new HttpRequestMessage(method, url);
        if (body != null)
        {
            request.Content = JsonContent.Create(body, options: JsonOptions);
        }

        if (_authStateProvider != null)
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            var token = authState.User.FindFirst(AdminAuthConstants.ApiTokenClaimType)?.Value;
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        return request;
    }

    protected async Task<T> GetAsync<T>(string url, CancellationToken ct = default)
        => await SendAsync<T>(async () => await Http.SendAsync(await BuildRequestAsync(HttpMethod.Get, url, null), ct));

    protected async Task<T> PostAsync<T>(string url, object? body = null, CancellationToken ct = default)
        => await SendAsync<T>(async () => await Http.SendAsync(await BuildRequestAsync(HttpMethod.Post, url, body), ct));

    protected async Task PostAsync(string url, object? body = null, CancellationToken ct = default)
        => await SendAsync(async () => await Http.SendAsync(await BuildRequestAsync(HttpMethod.Post, url, body), ct));

    protected async Task<T> PutAsync<T>(string url, object body, CancellationToken ct = default)
        => await SendAsync<T>(async () => await Http.SendAsync(await BuildRequestAsync(HttpMethod.Put, url, body), ct));

    private async Task<T> SendAsync<T>(Func<Task<HttpResponseMessage>> send)
    {
        HttpResponseMessage response;
        try
        {
            response = await send();
        }
        catch (HttpRequestException ex)
        {
            throw new ApiException($"تعذر الاتصال بالسيرفر: {ex.Message}");
        }

        var raw = await response.Content.ReadAsStringAsync();
        ApiResult<T>? result;
        try
        {
            result = string.IsNullOrWhiteSpace(raw)
                ? null
                : JsonSerializer.Deserialize<ApiResult<T>>(raw, JsonOptions);
        }
        catch (JsonException)
        {
            result = null;
        }

        if (!response.IsSuccessStatusCode || result is null || !result.Success)
        {
            throw new ApiException(
                result?.Message ?? $"خطأ غير متوقع ({(int)response.StatusCode})",
                (int)response.StatusCode,
                result?.Errors);
        }

        return result.Data!;
    }

    private async Task SendAsync(Func<Task<HttpResponseMessage>> send)
    {
        HttpResponseMessage response;
        try
        {
            response = await send();
        }
        catch (HttpRequestException ex)
        {
            throw new ApiException($"تعذر الاتصال بالسيرفر: {ex.Message}");
        }

        var raw = await response.Content.ReadAsStringAsync();
        ApiResult? result;
        try
        {
            result = string.IsNullOrWhiteSpace(raw)
                ? null
                : JsonSerializer.Deserialize<ApiResult>(raw, JsonOptions);
        }
        catch (JsonException)
        {
            result = null;
        }

        if (!response.IsSuccessStatusCode || result is null || !result.Success)
        {
            throw new ApiException(
                result?.Message ?? $"خطأ غير متوقع ({(int)response.StatusCode})",
                (int)response.StatusCode,
                result?.Errors);
        }
    }
}
