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
            "There is nothing to backup.",
            "MirrorVault",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }
}