using MirrorVault.Models;
using System.Management;

namespace MirrorVault.Services;

public class DriveDetectionService
{
    public List<BackupDriveInfo> GetExternalDrives()
    {
        List<BackupDriveInfo> drives = new();

        using var searcher = new ManagementObjectSearcher(
            @"SELECT * FROM Win32_DiskDrive WHERE InterfaceType='USB'");

        foreach (ManagementObject disk in searcher.Get())
        {
            foreach (ManagementObject partition in disk.GetRelated("Win32_DiskPartition"))
            {
                foreach (ManagementObject logicalDisk in partition.GetRelated("Win32_LogicalDisk"))
                {
                    string driveLetter = logicalDisk["DeviceID"]?.ToString() ?? string.Empty;
                    string volumeLabel = logicalDisk["VolumeName"]?.ToString() ?? string.Empty;

                    drives.Add(new BackupDriveInfo
                    {
                        DriveLetter = driveLetter,
                        VolumeLabel = volumeLabel
                    });
                }
            }
        }

        return drives;
    }
}