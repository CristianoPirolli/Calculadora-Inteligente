using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Windows;

namespace CalculadoraInteligente.Application.Updates;

public static class AutoUpdater
{
    private const string ConfigFileName = "update-config.json";

    public static async Task TryUpdateAsync(Window owner)
    {
        var config = await LoadConfigAsync();
        if (config is null || !config.Enabled || string.IsNullOrWhiteSpace(config.ManifestUrl))
            return;

        if (!Uri.TryCreate(config.ManifestUrl, UriKind.Absolute, out var manifestUri))
            return;

        using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(Math.Max(3, config.TimeoutSeconds)) };
        var manifestJson = await http.GetStringAsync(manifestUri);
        var manifest = JsonSerializer.Deserialize<UpdateManifest>(manifestJson, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (manifest is null || string.IsNullOrWhiteSpace(manifest.Version) || string.IsNullOrWhiteSpace(manifest.Url))
            return;

        if (!Version.TryParse(manifest.Version, out var latestVersion))
            return;

        var currentVersion = GetCurrentVersion();
        if (latestVersion <= currentVersion)
            return;

        if (config.PromptBeforeInstall)
        {
            var answer = MessageBox.Show(owner,
                $"Nova versão disponível: {latestVersion}\nVersão atual: {currentVersion}\n\nDeseja atualizar agora?",
                "Atualização disponível",
                MessageBoxButton.YesNo,
                MessageBoxImage.Information);

            if (answer != MessageBoxResult.Yes)
                return;
        }

        var installerPath = await DownloadInstallerAsync(http, manifest.Url, latestVersion);
        if (string.IsNullOrWhiteSpace(installerPath))
            return;

        StartInstallerAndExit(installerPath);
    }

    private static async Task<UpdateConfig?> LoadConfigAsync()
    {
        var filePath = Path.Combine(AppContext.BaseDirectory, ConfigFileName);
        if (!File.Exists(filePath))
            return null;

        var json = await File.ReadAllTextAsync(filePath);
        return JsonSerializer.Deserialize<UpdateConfig>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    private static Version GetCurrentVersion()
    {
        var entry = Assembly.GetEntryAssembly();
        return entry?.GetName().Version ?? new Version(1, 0, 0);
    }

    private static async Task<string?> DownloadInstallerAsync(HttpClient http, string installerUrl, Version latestVersion)
    {
        if (!Uri.TryCreate(installerUrl, UriKind.Absolute, out var installerUri))
            return null;

        var updatesDir = Path.Combine(Path.GetTempPath(), "MultiCalculos", "updates");
        Directory.CreateDirectory(updatesDir);

        var extension = Path.GetExtension(installerUri.LocalPath);
        if (string.IsNullOrWhiteSpace(extension))
            extension = ".exe";

        var installerPath = Path.Combine(updatesDir, $"MultiCalculos-Setup-{latestVersion}{extension}");

        await using var source = await http.GetStreamAsync(installerUri);
        await using var destination = File.Create(installerPath);
        await source.CopyToAsync(destination);

        return installerPath;
    }

    private static void StartInstallerAndExit(string installerPath)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = installerPath,
            Arguments = "/VERYSILENT /NORESTART /CLOSEAPPLICATIONS",
            UseShellExecute = true
        };

        Process.Start(startInfo);
        System.Windows.Application.Current?.Shutdown();
    }
}
