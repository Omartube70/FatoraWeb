using Fatora.Models.Api;
using Fatora.Services.Formatting;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace Fatora.Components.Admin.Shared;

// Shared request/error handling for every admin page: consistent loading flags,
// toast messages, and automatic sign-out on 401 - so this logic isn't repeated
// in every Clients/Plans/Licenses/Activations page.
public abstract class AdminComponentBase : ComponentBase, IDisposable
{
    [Inject] protected ISnackbar Snackbar { get; set; } = default!;
    [Inject] protected NavigationManager Nav { get; set; } = default!;
    [Inject] private DateService Clock { get; set; } = default!;
    [Inject] private IJSRuntime Js { get; set; } = default!;

    /// <summary>
    /// The admin's own time-zone offset in minutes (Cairo: 120 or 180). Only available once the page is
    /// interactive (the browser is asked), so call it from OnAfterRenderAsync, not from prerendering.
    /// </summary>
    protected async Task<int> GetTzOffsetAsync()
    {
        if (Clock.TzOffsetMinutes is { } known) return known;

        try
        {
            await using var module = await Js.InvokeAsync<IJSObjectReference>("import", "/admin.js");
            Clock.TzOffsetMinutes = await module.InvokeAsync<int>("getTimezoneOffset");
        }
        catch (Exception ex) when (ex is JSException or InvalidOperationException or JSDisconnectedException)
        {
            return 0;
        }

        return Clock.TzOffsetMinutes ?? 0;
    }

    // The browser's time zone arrives after the first render; times on the page are then redrawn in it.
    protected override void OnInitialized() => Clock.Changed += OnClockChanged;

    private void OnClockChanged() => _ = InvokeAsync(StateHasChanged);

    public virtual void Dispose() => Clock.Changed -= OnClockChanged;

    protected bool IsBusy { get; private set; }

    protected async Task<T?> RunAsync<T>(Func<Task<T>> action, bool showSuccessToast = false, string? successMessage = null)
    {
        if (IsBusy) return default;
        IsBusy = true;
        try
        {
            var result = await action();
            if (showSuccessToast)
                Snackbar.Add(successMessage ?? "تم بنجاح", Severity.Success);
            return result;
        }
        catch (ApiException ex)
        {
            HandleError(ex);
            return default;
        }
        finally
        {
            IsBusy = false;
            StateHasChanged();
        }
    }

    protected async Task<bool> RunAsync(Func<Task> action, bool showSuccessToast = true, string? successMessage = null)
    {
        if (IsBusy) return false;
        IsBusy = true;
        try
        {
            await action();
            if (showSuccessToast)
                Snackbar.Add(successMessage ?? "تم بنجاح", Severity.Success);
            return true;
        }
        catch (ApiException ex)
        {
            HandleError(ex);
            return false;
        }
        finally
        {
            IsBusy = false;
            StateHasChanged();
        }
    }

    protected void HandleError(ApiException ex)
    {
        switch (ex.StatusCode)
        {
            case 401:
                Nav.NavigateTo("/admin/account/logout", forceLoad: true);
                break;
            case 403:
                Nav.NavigateTo("/admin/access-denied");
                break;
            default:
                Snackbar.Add(ex.Message, Severity.Error);
                break;
        }
    }
}
