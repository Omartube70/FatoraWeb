using MudBlazor;

namespace Fatora;

/// <summary>
/// The admin panel's MudBlazor theme, in the colours of the public site (see the tokens at the top of
/// site.css). admin.css adds the rest of the look; fonts are set there too.
/// </summary>
public static class ThemeProvider
{
    public static MudTheme GetFatoraTheme()
    {
        var theme = new MudTheme();

        // Light palette
        theme.PaletteLight.Primary = "#6c4cf1";
        theme.PaletteLight.PrimaryContrastText = "#ffffff";
        theme.PaletteLight.Secondary = "#8b5cf6";
        theme.PaletteLight.Tertiary = "#12b886";
        theme.PaletteLight.Success = "#12b886";
        theme.PaletteLight.Info = "#2f9bff";
        theme.PaletteLight.Warning = "#f59f00";
        theme.PaletteLight.Error = "#f0568b";
        theme.PaletteLight.Background = "#fbfaff";
        theme.PaletteLight.Surface = "#ffffff";
        theme.PaletteLight.BackgroundGray = "#f2efff";
        theme.PaletteLight.AppbarBackground = "rgba(255,255,255,0.78)";
        theme.PaletteLight.AppbarText = "#14112b";
        theme.PaletteLight.DrawerBackground = "#ffffff";
        theme.PaletteLight.DrawerText = "#4b4768";
        theme.PaletteLight.DrawerIcon = "#7a7699";
        theme.PaletteLight.TextPrimary = "#14112b";
        theme.PaletteLight.TextSecondary = "#4b4768";
        theme.PaletteLight.TextDisabled = "rgba(20,17,43,0.38)";
        theme.PaletteLight.ActionDefault = "#7a7699";
        theme.PaletteLight.LinesDefault = "rgba(20,17,43,0.11)";
        theme.PaletteLight.LinesInputs = "rgba(20,17,43,0.22)";
        theme.PaletteLight.TableLines = "rgba(20,17,43,0.08)";
        theme.PaletteLight.TableStriped = "rgba(108,76,241,0.03)";
        theme.PaletteLight.TableHover = "rgba(108,76,241,0.06)";
        theme.PaletteLight.Divider = "rgba(20,17,43,0.11)";
        theme.PaletteLight.DividerLight = "rgba(20,17,43,0.06)";

        // Dark palette
        theme.PaletteDark.Primary = "#8b6dff";
        theme.PaletteDark.PrimaryContrastText = "#ffffff";
        theme.PaletteDark.Secondary = "#b583ff";
        theme.PaletteDark.Tertiary = "#2dd4a4";
        theme.PaletteDark.Success = "#2dd4a4";
        theme.PaletteDark.Info = "#5eb2ff";
        theme.PaletteDark.Warning = "#fbbf24";
        theme.PaletteDark.Error = "#ff7aa8";
        theme.PaletteDark.Background = "#0a0916";
        theme.PaletteDark.Surface = "#15122b";
        theme.PaletteDark.BackgroundGray = "#0e0c20";
        theme.PaletteDark.AppbarBackground = "rgba(10,9,22,0.72)";
        theme.PaletteDark.AppbarText = "#f4f2ff";
        theme.PaletteDark.DrawerBackground = "#0e0c20";
        theme.PaletteDark.DrawerText = "#bfbadf";
        theme.PaletteDark.DrawerIcon = "#8c87b3";
        theme.PaletteDark.TextPrimary = "#f4f2ff";
        theme.PaletteDark.TextSecondary = "#bfbadf";
        theme.PaletteDark.TextDisabled = "rgba(244,242,255,0.34)";
        theme.PaletteDark.ActionDefault = "#8c87b3";
        theme.PaletteDark.LinesDefault = "rgba(255,255,255,0.11)";
        theme.PaletteDark.LinesInputs = "rgba(255,255,255,0.24)";
        theme.PaletteDark.TableLines = "rgba(255,255,255,0.08)";
        theme.PaletteDark.TableStriped = "rgba(139,109,255,0.05)";
        theme.PaletteDark.TableHover = "rgba(139,109,255,0.1)";
        theme.PaletteDark.Divider = "rgba(255,255,255,0.11)";
        theme.PaletteDark.DividerLight = "rgba(255,255,255,0.06)";

        theme.LayoutProperties.DefaultBorderRadius = "14px";
        theme.LayoutProperties.DrawerWidthLeft = "272px";
        theme.LayoutProperties.DrawerWidthRight = "272px";
        theme.LayoutProperties.AppbarHeight = "68px";

        return theme;
    }
}
