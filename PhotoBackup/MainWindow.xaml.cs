using MirrorVault.Models;
using MirrorVault.Services;
using System.Windows;
using System;
using System.ComponentModel;

namespace MirrorVault;

public partial class MainWindow : Window
{
    private readonly BackupConfiguration _configuration;
    private readonly BackupService _backupService;
    private readonly UsbDetectionService _usbDetectionService;
    private readonly TrayIconService _trayIconService;

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

        StartupService startupService = new();

        if (_configuration.StartWithWindows)
        {
            startupService.EnableStartup(Environment.ProcessPath!);
        }
        else
        {
            startupService.DisableStartup();
        }

        _backupService = new BackupService(
            _configuration,
            robocopyService,
            notificationService);
        _usbDetectionService = new UsbDetectionService();
        _trayIconService = new TrayIconService();
        _trayIconService.ExitRequested += TrayIconService_ExitRequested;
        _trayIconService.OpenRequested += TrayIconService_OpenRequested;

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

    private void TrayIconService_OpenRequested()
    {
        Dispatcher.Invoke(() =>
        {
            Show();
            WindowState = WindowState.Normal;
            Activate();
        });
    }

    private void TrayIconService_ExitRequested()
    {
        _configuration.MinimizeToTray = false;

        Dispatcher.Invoke(() =>
        {
            Close();
        });
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        if (_configuration.MinimizeToTray)
        {
            e.Cancel = true;
            Hide();
            return;
        }

        _trayIconService.Dispose();

        base.OnClosing(e);
    }
}