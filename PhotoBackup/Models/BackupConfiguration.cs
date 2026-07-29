namespace MirrorVault.Models;

public class BackupConfiguration
{
    public string SourcePath { get; set; } = string.Empty;

    public string DestinationPath { get; set; } = string.Empty;

    public string DriveLabel { get; set; } = string.Empty;

    public bool DeleteRemovedFiles { get; set; } = true;

    public bool StartWithWindows { get; set; } = true;

    public bool MinimizeToTray { get; set; } = true;

    public bool RunBackupOnStartup { get; set; } = true;
}