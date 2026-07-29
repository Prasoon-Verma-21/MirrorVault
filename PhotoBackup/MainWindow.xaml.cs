using MirrorVault.Models;
using MirrorVault.Services;
using System.Windows;

namespace PhotoBackup;

public partial class MainWindow : Window
{
    private readonly BackupConfiguration _configuration;
    private readonly BackupService _backupService;
    private readonly UsbDetectionService _usbDetectionService;

    public MainWindow()
    {
        InitializeComponent();

        ConfigurationService configurationService = new();
        BackupConfiguration? savedConfiguration = configurationService.Load();

        if (savedConfiguration is not null)
        {
            _configuration = savedConfiguration;
        }
        else
        {
            _configuration = new BackupConfiguration
            {
                SourcePath = @"C:\Photos",
                DestinationPath = @"E:\Photos",
                DriveLabel = "MirrorVault"
            };

            configurationService.Save(_configuration);
        }

        configurationService.Save(_configuration);

        RobocopyService robocopyService = new();
        NotificationService notificationService = new();

        _backupService = new BackupService(
            _configuration,
            robocopyService,
            notificationService);
        _usbDetectionService = new UsbDetectionService();

        Loaded += MainWindow_Loaded;
    }

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        if (_usbDetectionService.IsBackupDriveConnected(_configuration.DriveLabel))
        {
            await _backupService.RunBackupAsync();
        }

        _usbDetectionService.BackupDriveConnected += UsbDetectionService_BackupDriveConnected;
        _usbDetectionService.StartListening(_configuration.DriveLabel);
    }

    private async void UsbDetectionService_BackupDriveConnected()
    {
        await Dispatcher.InvokeAsync(async () =>
        {
            await _backupService.RunBackupAsync();
        });
    }
}