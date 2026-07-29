using MirrorVault.Models;
using System.Diagnostics;

namespace MirrorVault.Services;

public class BackupService
{
    private readonly BackupConfiguration _configuration;
    private readonly RobocopyService _robocopyService;
    private readonly NotificationService _notificationService;

    public BackupService(
        BackupConfiguration configuration,
        RobocopyService robocopyService,
        NotificationService notificationService)
    {
        _configuration = configuration;
        _robocopyService = robocopyService;
        _notificationService = notificationService;
    }

    public async Task RunBackupAsync()
    {
        Debug.WriteLine("RunBackupAsync() called");

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

        if (_robocopyService.HasChanges(process.ExitCode))
            _notificationService.ShowBackupCompleted();
        else
            _notificationService.ShowNothingToBackup();
    }
}