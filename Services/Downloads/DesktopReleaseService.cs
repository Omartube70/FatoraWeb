using System.Text.Json.Serialization;

namespace Fatora.Services.Downloads;

public record DesktopRelease(string Version, string Name, DateTime PublishedAt, string PageUrl, string InstallerName, string InstallerUrl);

// The desktop installer's download links come from the newest GitHub release of the public
// releases repo, so publishing a new release is enough - the site never has to be edited to
// follow the version number. The list is cached (GitHub allows only 60 anonymous calls an
// hour per IP); when GitHub can't be reached the last good list is kept, and with none the
// pages fall back to the repo's "latest release" page.
public class DesktopReleaseService
{
    private static readonly TimeSpan CacheTime = TimeSpan.FromMinutes(10);
    private static readonly TimeSpan RetryTime = TimeSpan.FromMinutes(1);
    private const int MaxReleases = 10;

    private readonly HttpClient _http;
    private readonly string _repo;
    private readonly ILogger<DesktopReleaseService> _logger;
    private readonly SemaphoreSlim _lock = new(1, 1);
    private (IReadOnlyList<DesktopRelease> Releases, DateTime ValidUntil) _cache = ([], DateTime.MinValue);

    public DesktopReleaseService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<DesktopReleaseService> logger)
    {
        _http = httpClientFactory.CreateClient(nameof(DesktopReleaseService));
        _repo = configuration["Releases:Repository"]
            ?? throw new InvalidOperationException("Releases:Repository is not configured.");
        _logger = logger;
    }

    public string RepositoryUrl => $"https://github.com/{_repo}";

    public string LatestReleasePageUrl => $"{RepositoryUrl}/releases/latest";

    public async Task<DesktopRelease?> GetLatestAsync(CancellationToken ct = default)
        => (await GetReleasesAsync(ct)).FirstOrDefault();

    /// <summary>Published releases that have a Windows installer, newest first.</summary>
    public async Task<IReadOnlyList<DesktopRelease>> GetReleasesAsync(CancellationToken ct = default)
    {
        if (DateTime.UtcNow < _cache.ValidUntil) return _cache.Releases;

        await _lock.WaitAsync(ct);
        try
        {
            if (DateTime.UtcNow < _cache.ValidUntil) return _cache.Releases;

            try
            {
                _cache = (await FetchAsync(ct), DateTime.UtcNow + CacheTime);
            }
            catch (Exception ex) when (!ct.IsCancellationRequested)
            {
                _logger.LogWarning(ex, "Could not read the releases of {Repo} from GitHub.", _repo);
                _cache = (_cache.Releases, DateTime.UtcNow + RetryTime);
            }

            return _cache.Releases;
        }
        finally
        {
            _lock.Release();
        }
    }

    private async Task<IReadOnlyList<DesktopRelease>> FetchAsync(CancellationToken ct)
    {
        var releases = await _http.GetFromJsonAsync<List<GitHubRelease>>($"repos/{_repo}/releases?per_page={MaxReleases}", ct)
            ?? [];

        var result = new List<DesktopRelease>();
        foreach (var release in releases.Where(r => !r.Draft && !r.Prerelease).OrderByDescending(r => r.PublishedAt))
        {
            // Same pick as the desktop auto-updater: the .exe whose name mentions "fatora" (a release
            // can also carry other .exe files, e.g. the SQL Server Express installer).
            var installer = release.Assets.FirstOrDefault(a =>
                a.Name.EndsWith(".exe", StringComparison.OrdinalIgnoreCase) &&
                a.Name.Contains("fatora", StringComparison.OrdinalIgnoreCase));
            if (installer is null) continue;

            var version = release.TagName.TrimStart('v', 'V');
            result.Add(new DesktopRelease(version, release.Name ?? $"v{version}", release.PublishedAt, release.HtmlUrl, installer.Name, installer.DownloadUrl));
        }

        return result;
    }

    private sealed class GitHubRelease
    {
        [JsonPropertyName("tag_name")] public string TagName { get; set; } = "";
        [JsonPropertyName("name")] public string? Name { get; set; }
        [JsonPropertyName("html_url")] public string HtmlUrl { get; set; } = "";
        [JsonPropertyName("published_at")] public DateTime PublishedAt { get; set; }
        [JsonPropertyName("draft")] public bool Draft { get; set; }
        [JsonPropertyName("prerelease")] public bool Prerelease { get; set; }
        [JsonPropertyName("assets")] public List<GitHubAsset> Assets { get; set; } = [];
    }

    private sealed class GitHubAsset
    {
        [JsonPropertyName("name")] public string Name { get; set; } = "";
        [JsonPropertyName("browser_download_url")] public string DownloadUrl { get; set; } = "";
    }
}
