using MirrorVault.Models;

namespace MirrorVault.Services;

public class BackupService
{
    private readonly BackupConfiguration _configuration;

    public BackupService(BackupConfiguration configuration)
    {
        _configuration = configuration;
    }
}