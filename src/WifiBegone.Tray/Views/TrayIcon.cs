namespace WifiBegone.Tray.Views
{
    using System;
    using System.Reflection;
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
            _icon.Icon = Properties.Resources.Icon;
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
                new MenuItem($"WifiBegone v{Assembly.GetExecutingAssembly().GetName().Version}") {Enabled = false},
                new MenuItem("Show/Hide Debug Console", ShowHidDebugConsoleClick),
                new MenuItem("Check for Updates", CheckForUpdatesClick),
                new MenuItem("Exit", ExitClick),
            };
            var menu = new ContextMenu(items);
            return menu;
        }

        private void CheckForUpdatesClick(object sender, EventArgs eventArgs)
        {
            ConsoleManager.Show();
            Task.Run(async () => await SquirrelManager.UpdateAsync())
                .ContinueWith(task => ConsoleManager.Hide());
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