using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fatora.Models.Api;

/// <summary>
/// What a shop's app may switch on, as stored in License.Features (JSON). The keys are exactly
/// the desktop's own feature switches (Fatora_DAL BusinessSettings / Fatora_BLL FeatureGuard),
/// so the app can apply the list straight to its settings: what is not in <see cref="Allowed"/>
/// shows locked and stays off, and what IS in it the shop may turn on and off as it likes.
/// A license with no Features at all (null) gets everything — every license issued before this
/// existed keeps working exactly as it did.
/// </summary>
public class LicenseFeatures
{
    /// <summary>"Any" = the shop chooses in Settings; "Barcode" / "Touch" = fixed by the license.</summary>
    [JsonPropertyName("cashierMode")]
    public string CashierMode { get; set; } = CashierModeAny;

    [JsonPropertyName("allowed")]
    public List<string> Allowed { get; set; } = new();

    public const string CashierModeAny = "Any";
    public const string CashierModeBarcode = "Barcode";
    public const string CashierModeTouch = "Touch";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    /// <summary>The switches, in the groups the desktop's Settings shows them in.</summary>
    public static readonly (string Group, (string Key, string Label)[] Items)[] Catalogue =
    {
        ("عام", new[]
        {
            ("CustomerManagement", "العملاء"),
            ("SupplierManagement", "الموردين"),
            ("Inventory", "المخزون"),
            ("BarcodePrinting", "طباعة الباركود"),
            ("BarcodeScanner", "قارئ الباركود"),
            ("ThermalPrinter", "الطابعة الحرارية"),
            ("Discounts", "الخصومات والعروض"),
            ("Taxes", "الضرائب"),
            ("BackupRestore", "النسخ الاحتياطي والاستعادة"),
            ("Installments", "التقسيط"),
            ("EWalletTransfers", "المحافظ الإلكترونية")
        }),
        ("المطاعم والكافيهات", new[]
        {
            ("TablesManagement", "الطاولات"),
            ("DineIn", "صالة"),
            ("TakeAway", "تيك أواي"),
            ("Delivery", "التوصيل"),
            ("CustomerPhoneLookup", "البحث برقم العميل"),
            ("KitchenNotes", "ملاحظات المطبخ"),
            ("KitchenPrinter", "طابعة المطبخ")
        }),
        ("المحلات", new[]
        {
            ("WalkInCustomer", "عميل نقدي"),
            ("CustomerSelection", "اختيار العميل في البيع"),
            ("QuickCustomerCreation", "إنشاء عميل سريع"),
            ("RetailDelivery", "توصيل الطلبات")
        }),
        ("الموظفين", new[]
        {
            ("Attendance", "الحضور والانصراف"),
            ("ShiftManagement", "الورديات"),
            ("UserPermissions", "صلاحيات المستخدمين")
        }),
        ("التقارير", new[]
        {
            ("SalesReports", "تقارير المبيعات"),
            ("InventoryReports", "تقارير المخزون"),
            ("CustomerReports", "تقارير العملاء"),
            ("SupplierReports", "تقارير الموردين"),
            ("AttendanceReports", "تقارير الحضور")
        })
    };

    public static IEnumerable<string> AllKeys => Catalogue.SelectMany(g => g.Items).Select(i => i.Key);

    /// <summary>The desktop's own Retail preset (FeatureGuard.ApplyRetailPreset).</summary>
    public static readonly string[] RetailPreset =
    {
        "CustomerManagement", "SupplierManagement", "Inventory", "BarcodePrinting", "BarcodeScanner",
        "ThermalPrinter", "Discounts", "BackupRestore", "WalkInCustomer", "CustomerSelection",
        "QuickCustomerCreation", "Attendance", "ShiftManagement", "UserPermissions",
        "SalesReports", "InventoryReports", "CustomerReports", "SupplierReports"
    };

    /// <summary>The desktop's own Restaurant preset.</summary>
    public static readonly string[] RestaurantPreset =
    {
        "CustomerManagement", "SupplierManagement", "Inventory", "ThermalPrinter", "Discounts",
        "BackupRestore", "TablesManagement", "DineIn", "TakeAway", "Delivery", "CustomerPhoneLookup",
        "KitchenNotes", "Attendance", "ShiftManagement", "UserPermissions", "SalesReports", "InventoryReports"
    };

    /// <summary>null / blank (an older license) = everything is allowed.</summary>
    public static LicenseFeatures Parse(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return new LicenseFeatures { Allowed = AllKeys.ToList() };

        try
        {
            var parsed = JsonSerializer.Deserialize<LicenseFeatures>(json, JsonOptions);
            if (parsed == null)
                return new LicenseFeatures { Allowed = AllKeys.ToList() };

            parsed.Allowed ??= new List<string>();
            if (string.IsNullOrWhiteSpace(parsed.CashierMode))
                parsed.CashierMode = CashierModeAny;

            return parsed;
        }
        catch (JsonException)
        {
            return new LicenseFeatures { Allowed = AllKeys.ToList() };
        }
    }

    /// <summary>Everything on and the shop's own choice of cashier mode = no restriction: send null.</summary>
    public string? ToJsonOrNull()
    {
        bool everything = CashierMode == CashierModeAny && AllKeys.All(k => Allowed.Contains(k));

        return everything ? null : JsonSerializer.Serialize(this, JsonOptions);
    }
}
