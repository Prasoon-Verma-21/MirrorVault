namespace MirrorVault.Models;

public class BackupDriveInfo
{
    public string DriveLetter { get; set; } = string.Empty;

    public string VolumeLabel { get; set; } = string.Empty;

    public string DisplayName =>
        string.IsNullOrWhiteSpace(VolumeLabel)
            ? DriveLetter
            : $"{VolumeLabel} ({DriveLetter})";
}