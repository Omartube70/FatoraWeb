using MudBlazor;

namespace Fatora;

public static class ThemeProvider
{
    public static MudTheme GetFatoraTheme()
    {
        var theme = new MudTheme();
        
        // Light palette
        theme.PaletteLight.Primary = "#6C4CF1";
        theme.PaletteLight.Secondary = "#8B5CF6";
        theme.PaletteLight.Success = "#22C55E";
        theme.PaletteLight.AppbarBackground = "#FFFFFF";
        theme.PaletteLight.AppbarText = "#1F2937";
        theme.PaletteLight.DrawerBackground = "#FFFFFF";
        theme.PaletteLight.DrawerText = "#1F2937";
        theme.PaletteLight.Error = "#EF4444";
        theme.PaletteLight.Info = "#3B82F6";
        theme.PaletteLight.Warning = "#F59E0B";
        theme.PaletteLight.Surface = "#FFFFFF";
        theme.PaletteLight.Background = "#F8F9FC";
        
        // Dark palette
        theme.PaletteDark.Primary = "#8B5CF6";
        theme.PaletteDark.Secondary = "#A78BFA";
        theme.PaletteDark.Success = "#22C55E";
        theme.PaletteDark.AppbarBackground = "#1E293B";
        theme.PaletteDark.AppbarText = "#F1F5F9";
        theme.PaletteDark.DrawerBackground = "#1E293B";
        theme.PaletteDark.DrawerText = "#F1F5F9";
        theme.PaletteDark.Error = "#F87171";
        theme.PaletteDark.Info = "#60A5FA";
        theme.PaletteDark.Warning = "#FBBF24";
        theme.PaletteDark.Surface = "#1E293B";
        theme.PaletteDark.Background = "#0F172A";

        return theme;
    }
}
