using System.Drawing;
using System.Windows.Forms;

namespace MirrorVault.Services;

public class TrayIconService : IDisposable
{
    private readonly NotifyIcon _notifyIcon;

    public event Action? ExitRequested;
    public event Action? OpenRequested;

    public TrayIconService()
    {
        ContextMenuStrip menu = new();

        menu.Items.Add("Open", null, (_, _) =>
        {
            OpenRequested?.Invoke();
        });
        menu.Items.Add(new ToolStripSeparator());

        menu.Items.Add("Exit", null, (_, _) =>
        {
            ExitRequested?.Invoke();
        });

        _notifyIcon = new NotifyIcon
        {
            Icon = SystemIcons.Application,
            Text = "MirrorVault",
            ContextMenuStrip = menu,
            Visible = true
        };
    }

    public void Dispose()
    {
        _notifyIcon.Visible = false;
        _notifyIcon.Dispose();
    }
}