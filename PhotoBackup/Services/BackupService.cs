using MirrorVault.Models;
using System.Diagnostics;

namespace MirrorVault.Services;

public class BackupService
{
    private readonly BackupConfiguration _configuration;
    private readonly RobocopyService _robocopyService;
    private readonly UsbDetectionService _usbDetectionService;
    private readonly NotificationService _notificationService;

    public BackupService(BackupConfiguration configuration)
    {
        _configuration = configuration;
        _robocopyService = new RobocopyService();
        _usbDetectionService = new UsbDetectionService();
        _notificationService = new NotificationService();
    }

    public async Task RunBackupAsync()
    {
        if (!_usbDetectionService.IsBackupDriveConnected(_configuration.DriveLabel))
            return;

        _notificationService.ShowBackupStarted();

        ProcessStartInfo startInfo = _robocopyService.CreateProcessStartInfo(
            _configuration.SourcePath,
            _configuration.DestinationPath);

        using Process process = new()
        {
            StartInfo = startInfo
        };

        process.Start();

        await process.WaitForExitAsync();

        _notificationService.ShowBackupCompleted();
    }
}