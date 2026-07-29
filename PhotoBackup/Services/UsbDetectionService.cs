using System.Management;

namespace MirrorVault.Services;

public class UsbDetectionService
{
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