namespace MirrorVault.Models;

public class BackupConfiguration
{
    public string SourcePath { get; set; } = string.Empty;

    public string DestinationPath { get; set; } = string.Empty;

    public string DriveLabel { get; set; } = string.Empty;
}