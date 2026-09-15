using Fatora.Models.Api;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Fatora.Components.Admin.Shared;

// Shared request/error handling for every admin page: consistent loading flags,
// toast messages, and automatic sign-out on 401 - so this logic isn't repeated
// in every Clients/Plans/Licenses/Activations page.
public abstract class AdminComponentBase : ComponentBase
{
    [Inject] protected ISnackbar Snackbar { get; set; } = default!;
    [Inject] protected NavigationManager Nav { get; set; } = default!;

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

    private void HandleError(ApiException ex)
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
