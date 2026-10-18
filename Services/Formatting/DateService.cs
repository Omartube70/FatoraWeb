namespace Fatora.Services.Formatting;

/// <summary>
/// Dates for the admin panel. The license server stores UTC; the panel shows the clock of the admin's
/// own browser. The browser's offset arrives after the first render (<see cref="TzOffsetMinutes"/>); until
/// then the web server's own zone is used, and every admin page re-renders once the offset is known.
/// Scoped (one per circuit) because the offset belongs to one browser.
/// </summary>
public class DateService
{
    private int? _tzOffsetMinutes;

    /// <summary>Raised when the browser's offset becomes known, so open pages can re-render.</summary>
    public event Action? Changed;

    /// <summary>Minutes the admin's clock is ahead of UTC (Cairo: 120 or 180); null = not known yet.</summary>
    public int? TzOffsetMinutes
    {
        get => _tzOffsetMinutes;
        set
        {
            if (_tzOffsetMinutes == value) return;
            _tzOffsetMinutes = value;
            Changed?.Invoke();
        }
    }

    /// <summary>A server timestamp (UTC; "unspecified" kind means UTC) on the admin's clock.</summary>
    public DateTime ToLocal(DateTime value)
    {
        var utc = value.Kind == DateTimeKind.Local ? value.ToUniversalTime() : DateTime.SpecifyKind(value, DateTimeKind.Utc);
        return _tzOffsetMinutes is { } minutes ? utc.AddMinutes(minutes) : utc.ToLocalTime();
    }

    /// <summary>Today on the admin's clock.</summary>
    public DateTime LocalNow => ToLocal(DateTime.UtcNow);

    public string FormatDate(DateTime value) => ToLocal(value).ToString("yyyy/MM/dd");

    public string FormatDateTime(DateTime value) => ToLocal(value).ToString("yyyy/MM/dd HH:mm");

    public string FormatTime(DateTime value) => ToLocal(value).ToString("HH:mm");

    public string FormatDate(DateTime? value) => value.HasValue ? FormatDate(value.Value) : "—";

    public string FormatDateTime(DateTime? value) => value.HasValue ? FormatDateTime(value.Value) : "—";

    /// <summary>A calendar day that the server already cut in the admin's zone (the daily series) — no conversion.</summary>
    public string FormatDay(DateTime day) => day.ToString("dd/MM");

    public string FormatRelative(DateTime? value)
    {
        if (!value.HasValue) return "—";
        var utc = value.Value.Kind == DateTimeKind.Local ? value.Value.ToUniversalTime() : DateTime.SpecifyKind(value.Value, DateTimeKind.Utc);
        var diff = DateTime.UtcNow - utc;

        if (diff.TotalDays >= 2) return "منذ " + Plural((int)diff.TotalDays, "يوم", "يومين", "أيام", "يوماً");
        if (diff.TotalDays >= 1) return "أمس";
        if (diff.TotalHours >= 1) return "منذ " + Plural((int)diff.TotalHours, "ساعة", "ساعتين", "ساعات", "ساعة");
        if (diff.TotalMinutes >= 1) return "منذ " + Plural((int)diff.TotalMinutes, "دقيقة", "دقيقتين", "دقائق", "دقيقة");
        return "الآن";
    }

    /// <summary>Arabic counted nouns: one, two, 3–10 (few) and 11+ (many).</summary>
    private static string Plural(int n, string one, string two, string few, string many) => n switch
    {
        1 => one,
        2 => two,
        >= 3 and <= 10 => $"{n} {few}",
        _ => $"{n} {many}"
    };

    /// <summary>"in 5 days" for a date in the future (license expiry).</summary>
    public string FormatDaysLeft(int days) => days switch
    {
        <= 0 => "ينتهي اليوم",
        1 => "غداً",
        2 => "بعد يومين",
        <= 10 => $"بعد {days} أيام",
        _ => $"بعد {days} يوماً"
    };
}
