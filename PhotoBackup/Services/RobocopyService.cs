using MirrorVault.Helpers;
using System.Diagnostics;

namespace MirrorVault.Services;

public class RobocopyService
{
    public string BuildArguments(string source, string destination)
    {
        return $"\"{source}\" \"{destination}\" " +
               $"{RobocopyArguments.Mirror} " +
               $"{RobocopyArguments.CopyDataAttributesTimestamps} " +
               $"{RobocopyArguments.RetryCount} " +
               $"{RobocopyArguments.WaitTime} " +
               $"{RobocopyArguments.NoProgress} " +
               $"{RobocopyArguments.NoFileList} " +
               $"{RobocopyArguments.NoDirectoryList}";
    }

    public ProcessStartInfo CreateProcessStartInfo(string source, string destination)
    {
        return new ProcessStartInfo
        {
            FileName = "robocopy",
            Arguments = BuildArguments(source, destination),
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
    }

    public bool HasChanges(int exitCode)
    {
        return exitCode >= 1 && exitCode <= 7;
    }
}