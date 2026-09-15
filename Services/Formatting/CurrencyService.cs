using System.Globalization;

namespace Fatora.Services.Formatting;

public class CurrencyService
{
    public string CurrencyCode { get; set; } = "EGP";
    public string CurrencySymbol { get; set; } = "ج.م";

    public string Format(decimal amount)
        => $"{amount.ToString("N2", CultureInfo.InvariantCulture)} {CurrencySymbol}";
}
