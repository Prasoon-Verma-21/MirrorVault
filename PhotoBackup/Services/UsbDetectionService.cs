using System.Management;

namespace MirrorVault.Services;

public class UsbDetectionService
{
    public event Action? BackupDriveConnected;

    public void StartListening(string driveLabel)
    {
        WqlEventQuery query = new(
            "SELECT * FROM Win32_VolumeChangeEvent WHERE EventType = 2");

        ManagementEventWatcher watcher = new(query);

        watcher.EventArrived += (_, _) =>
        {
            if (IsBackupDriveConnected(driveLabel))
            {
                BackupDriveConnected?.Invoke();
            }
        };

        watcher.Start();
    }

    public bool IsBackupDriveConnected(string driveLabel)
    {
        using ManagementObjectSearcher searcher =
            new("SELECT * FROM Win32_LogicalDisk WHERE DriveType = 2");

        foreach (ManagementObject drive in searcher.Get())
        {
            string? volumeName = drive["VolumeName"]?.ToString();

            if (string.Equals(volumeName, driveLabel, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }
}