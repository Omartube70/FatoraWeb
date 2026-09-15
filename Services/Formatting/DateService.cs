namespace Fatora.Services.Formatting;

public class DateService
{
    public string FormatDate(DateTime value) => value.ToLocalTime().ToString("yyyy/MM/dd");

    public string FormatDateTime(DateTime value) => value.ToLocalTime().ToString("yyyy/MM/dd HH:mm");

    public string FormatDate(DateTime? value) => value.HasValue ? FormatDate(value.Value) : "—";

    public string FormatDateTime(DateTime? value) => value.HasValue ? FormatDateTime(value.Value) : "—";

    public string FormatRelative(DateTime? value)
    {
        if (!value.HasValue) return "—";
        var diff = DateTime.UtcNow - value.Value.ToUniversalTime();
        if (diff.TotalDays > 1) return $"منذ {(int)diff.TotalDays} يوم";
        if (diff.TotalHours > 1) return $"منذ {(int)diff.TotalHours} ساعة";
        if (diff.TotalMinutes > 1) return $"منذ {(int)diff.TotalMinutes} دقيقة";
        return "الآن";
    }
}
