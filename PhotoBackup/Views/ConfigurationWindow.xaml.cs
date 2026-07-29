using MirrorVault.Models;
using MirrorVault.Services;
using System.IO;
using System.Windows;
using WinForms = System.Windows.Forms;

namespace MirrorVault.Views;

public partial class ConfigurationWindow : Window
{
    public ConfigurationWindow()
    {
        InitializeComponent();

        BackupDriveComboBox.DisplayMemberPath = nameof(BackupDriveInfo.DisplayName);
    }

    private void BrowseButton_Click(object sender, RoutedEventArgs e)
    {
        using var dialog = new WinForms.FolderBrowserDialog();

        dialog.Description = "Select the folder containing your photos";
        dialog.UseDescriptionForTitle = true;

        if (dialog.ShowDialog() == WinForms.DialogResult.OK)
        {
            SourceFolderTextBox.Text = dialog.SelectedPath;
        }
    }

    private void ConfigurationWindow_Loaded(object sender, RoutedEventArgs e)
    {
        BackupDriveComboBox.Items.Clear();

        foreach (DriveInfo drive in DriveInfo.GetDrives())
        {
            if (!drive.IsReady)
                continue;

            // Skip the Windows system drive
            if (string.Equals(drive.Name, @"C:\", StringComparison.OrdinalIgnoreCase))
                continue;

            // Skip drives without a volume label
            if (string.IsNullOrWhiteSpace(drive.VolumeLabel))
                continue;

            BackupDriveComboBox.Items.Add(new BackupDriveInfo
            {
                DriveLetter = drive.Name.TrimEnd('\\'),
                VolumeLabel = drive.VolumeLabel
            });
        }

        if (BackupDriveComboBox.Items.Count > 0)
        {
            BackupDriveComboBox.SelectedIndex = 0;
        }
    }
    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (BackupDriveComboBox.SelectedItem is not BackupDriveInfo selectedDrive)
        {
            System.Windows.MessageBox.Show("Please select a backup drive.");
            return;
        }

        BackupConfiguration configuration = new()
        {
            SourcePath = SourceFolderTextBox.Text,
            DestinationPath = Path.Combine(selectedDrive.DriveLetter + @"\", "MirrorVault"),
            DriveLabel = selectedDrive.VolumeLabel
        };

        ConfigurationService configurationService = new();

        configurationService.Save(configuration);
    }
}