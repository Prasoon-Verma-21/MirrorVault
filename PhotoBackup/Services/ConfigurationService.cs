using MirrorVault.Models;
using System.IO;
using System.Text.Json;

namespace MirrorVault.Services;

public class ConfigurationService
{
    private const string FileName = "config.json";

    public void Save(BackupConfiguration configuration)
    {
        string json = JsonSerializer.Serialize(configuration, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(FileName, json);
    }

    public BackupConfiguration? Load()
    {
        if (!File.Exists(FileName))
            return null;

        string json = File.ReadAllText(FileName);

        return JsonSerializer.Deserialize<BackupConfiguration>(json);
    }
}