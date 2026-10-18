namespace Fatora.Components.Admin.Shared;

/// <summary>Arabic texts for the raw values the license server sends (shared by the dashboard, the devices page and the log).</summary>
public static class AdminLabels
{
    private static readonly Dictionary<string, string> Actions = new()
    {
        ["AdminLogin"] = "تسجيل دخول المدير",
        ["CreateClient"] = "إنشاء عميل",
        ["UpdateClient"] = "تعديل عميل",
        ["BlockClient"] = "حظر عميل",
        ["UnblockClient"] = "إلغاء حظر عميل",
        ["CreateLicense"] = "إنشاء ترخيص",
        ["UpdateLicense"] = "تعديل ترخيص",
        ["ActivateLicense"] = "تفعيل ترخيص",
        ["SuspendLicense"] = "إيقاف ترخيص",
        ["RevokeLicense"] = "إلغاء ترخيص",
        ["RenewLicense"] = "تجديد ترخيص",
        ["DeactivateDevice"] = "إلغاء تفعيل جهاز",
        ["ValidateLicense"] = "تحقق من ترخيص",
        ["ActivateDevice"] = "تفعيل جهاز",
        ["CreatePlan"] = "إنشاء باقة",
        ["UpdatePlan"] = "تعديل باقة",
        ["DeactivatePlan"] = "تعطيل باقة",
        ["ClientSelfRegister"] = "تسجيل عميل ذاتي",
        ["ChangeAdminPassword"] = "تغيير كلمة مرور المدير",
        ["ExpiryNotificationSent"] = "إشعار قرب الانتهاء",
        ["SyncDeviceRegistered"] = "ربط جهاز بالمزامنة",
        ["SyncUserLogin"] = "دخول مستخدم عبر المزامنة",
        ["SyncUpload"] = "رفع بيانات مزامنة",
        ["SyncDownload"] = "تنزيل بيانات مزامنة",
        ["SyncConflict"] = "تعارض مزامنة",
        ["SyncConflictResolved"] = "حل تعارض مزامنة",
        ["SyncDeviceDisabled"] = "إيقاف جهاز مزامنة",
        ["SyncDeviceEnabled"] = "تشغيل جهاز مزامنة",
        ["SyncBootstrap"] = "تهيئة مستخدمي المحل",
    };

    public static string Action(string action) => Actions.TryGetValue(action, out var label) ? label : action;

    /// <summary>violet | teal | amber | sky | rose — the avatar tone of an audit-log action.</summary>
    public static string ActionTone(string action) => action switch
    {
        "BlockClient" or "RevokeLicense" or "DeactivateDevice" or "DeactivatePlan" or "SyncDeviceDisabled" => "rose",
        "SuspendLicense" or "SyncConflict" or "ExpiryNotificationSent" => "amber",
        "UnblockClient" or "ActivateLicense" or "ActivateDevice" or "CreatePlan" or "RenewLicense"
            or "CreateClient" or "CreateLicense" or "SyncDeviceRegistered" or "SyncDeviceEnabled" or "ClientSelfRegister" => "teal",
        "AdminLogin" or "SyncUserLogin" => "sky",
        _ => "violet"
    };

    public static string Platform(string platform) => platform switch
    {
        "Desktop" => "ويندوز",
        "Mobile" => "موبايل",
        "Both" => "ويندوز + موبايل",
        _ => platform
    };

    public static string LicenseStatus(string status) => status switch
    {
        "Active" => "نشطة",
        "Pending" => "قيد الانتظار",
        "Expired" => "منتهية",
        "Suspended" => "موقوفة",
        "Revoked" => "ملغاة",
        _ => status
    };

    public static string DeviceStatus(string status) => status switch
    {
        "Active" => "نشط",
        "Inactive" => "غير نشط",
        "Revoked" => "ملغي",
        _ => status
    };

    public static string RequestKind(string kind) => kind switch
    {
        "License" => "التراخيص والتفعيل",
        "Sync" => "المزامنة",
        "Admin" => "لوحة الإدارة",
        "Other" => "أخرى",
        _ => kind
    };

    /// <summary>How recently a copy was used, for the colour of its dot: live | day | week | old | never.</summary>
    public static string Freshness(DateTime? lastActivityUtc, bool isOnline)
    {
        if (isOnline) return "live";
        if (lastActivityUtc is not { } t) return "never";

        var utc = t.Kind == DateTimeKind.Local ? t.ToUniversalTime() : DateTime.SpecifyKind(t, DateTimeKind.Utc);
        var age = DateTime.UtcNow - utc;
        if (age.TotalHours <= 24) return "day";
        if (age.TotalDays <= 7) return "week";
        return "old";
    }
}
