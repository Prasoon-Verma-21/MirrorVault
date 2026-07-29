using MirrorVault.Models;
using MirrorVault.Services;
using System.Windows;

namespace PhotoBackup;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        Loaded += MainWindow_Loaded;
    }

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        BackupConfiguration configuration = new()
        {
            SourcePath = @"C:\Photos",
            DestinationPath = @"E:\Photos",
            DriveLabel = "MirrorVault"
        };

        BackupService backupService = new(configuration);

        await backupService.RunBackupAsync();
    }
}