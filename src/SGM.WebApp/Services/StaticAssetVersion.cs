using System.Collections.Concurrent;
using System.Security.Cryptography;

namespace SGM.WebApp.Services;

/// <summary>
/// Appends a content hash to static asset URLs (<c>resume.pdf?v=1a2b3c4d</c>) so a
/// republished file gets a new URL and browsers and CDNs stop serving the old copy.
/// </summary>
public sealed class StaticAssetVersion(IWebHostEnvironment env)
{
    private readonly ConcurrentDictionary<string, (DateTime Modified, string Hash)> _cache = new();

    /// <param name="relativePath">Path under wwwroot, e.g. <c>resume.pdf</c>.</param>
    public string Url(string relativePath)
    {
        var file = env.WebRootFileProvider.GetFileInfo(relativePath);
        if (!file.Exists || file.PhysicalPath is null)
        {
            return relativePath;
        }

        var modified = file.LastModified.UtcDateTime;
        var entry = _cache.GetOrAdd(relativePath, _ => (modified, Hash(file.PhysicalPath)));
        if (entry.Modified != modified)
        {
            entry = (modified, Hash(file.PhysicalPath));
            _cache[relativePath] = entry;
        }

        return $"{relativePath}?v={entry.Hash}";
    }

    private static string Hash(string path)
    {
        using var stream = File.OpenRead(path);
        return Convert.ToHexStringLower(SHA256.HashData(stream))[..12];
    }
}
