using Microsoft.AspNetCore.Components;

namespace Fatora.Components.Site;

/// <summary>
/// Base class of the public site's components. The current language is cascaded down from
/// MainLayout; because it is a plain string, every component re-renders when it changes.
/// </summary>
public abstract class FxComponentBase : ComponentBase
{
    [CascadingParameter(Name = "FxLang")]
    public string Lang { get; set; } = SiteLanguage.Arabic;

    protected bool Ar => Lang == SiteLanguage.Arabic;

    /// <summary>Picks the Arabic or English text for the active language.</summary>
    protected string T(string ar, string en) => Ar ? ar : en;
}
