namespace WifiBegone.Tray.Views
{
    using System;
    using System.Drawing;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using WifiBegone.Tray.Helpers;
    using WifiBegone.Tray.Service;

    using Application = System.Windows.Application;

    public class TrayIcon
    {
        private readonly NotifyIcon _icon;

        public TrayIcon()
        {
            _icon = new NotifyIcon();
        }

        public void Show()
        {
            _icon.Icon = SystemIcons.WinLogo;
            _icon.Visible = true;
            _icon.ContextMenu = CreateContextMenu();
        }

        public void Notification(string message)
        {
            _icon.ShowBalloonTip(1000 * 2, null, message, ToolTipIcon.Info);
        }

        private ContextMenu CreateContextMenu()
        {
            var items = new[]
            {
                new MenuItem("Show/Hide Debug Console", ShowHidDebugConsoleClick),
                new MenuItem("Check for Updates", CheckForUpdatesClick),
                new MenuItem("Exit", ExitClick),
            };
            var menu = new ContextMenu(items);
            return menu;
        }

        private void CheckForUpdatesClick(object sender, EventArgs eventArgs)
        {
            Task.Run(async () => await SquirrelManager.UpdateAsync());
        }

        private void ExitClick(object sender, EventArgs eventArgs)
        {
            Application.Current.Shutdown();
        }

        private void ShowHidDebugConsoleClick(object sender, EventArgs eventArgs)
        {
            ConsoleManager.Toggle();
            Console.WriteLine("Logging started. Use tray menu to hide again.\n\n");
        }
    }
}