using System.Windows;

namespace MirrorVault.Services;

public class NotificationService
{
    public void ShowBackupStarted()
    {
        MessageBox.Show(
            "Backup has started.",
            "MirrorVault",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    public void ShowBackupCompleted()
    {
        MessageBox.Show(
            "Backup completed successfully.",
            "MirrorVault",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    public void ShowNothingToBackup()
    {
        MessageBox.Show(
            "No changes detected. Backup is already up to date.",
            "MirrorVault",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }
}