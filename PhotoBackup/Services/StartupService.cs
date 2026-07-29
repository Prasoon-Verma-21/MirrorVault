using Microsoft.Win32;

namespace MirrorVault.Services;

public class StartupService
{
    private const string AppName = "MirrorVault";

    public void EnableStartup(string executablePath)
    {
        using RegistryKey? key = Registry.CurrentUser.OpenSubKey(
            @"Software\Microsoft\Windows\CurrentVersion\Run",
            true);

        key?.SetValue(AppName, executablePath);
    }

    public void DisableStartup()
    {
        using RegistryKey? key = Registry.CurrentUser.OpenSubKey(
            @"Software\Microsoft\Windows\CurrentVersion\Run",
            true);

        key?.DeleteValue(AppName, false);
    }
}