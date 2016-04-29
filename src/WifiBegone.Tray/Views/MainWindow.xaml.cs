namespace WifiBegone.Tray.Views
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows;

    using Squirrel;

    using WifiBegone.Tray.Service;

    public partial class MainWindow : Window
    {
        private TrayIcon _tray;
        private BackgroundService _backgroundService;

        public MainWindow()
        {
            InitializeComponent();

            Install();

            _tray = new TrayIcon();
            _tray.Show();

            var trayLogger = new TrayLogger(_tray);
            _backgroundService = new BackgroundService(trayLogger);

            _backgroundService.Start();
        }

        private const string UpdateUrl = "D:\\Development\\WifiBegone\\Releases";

        private void Install()
        {
            try
            {
                //using (var mgr = new UpdateManager(UpdateUrl))
                //{
                //    // Note, in most of these scenarios, the app exits after this method
                //    // completes!

                //    var exePath = Path.GetFileName(Assembly.GetEntryAssembly().Location);
                //    const ShortcutLocation shortcutPaths =
                //        ShortcutLocation.StartMenu | ShortcutLocation.Startup | ShortcutLocation.Desktop;

                //    SquirrelAwareApp.HandleEvents(
                //        onInitialInstall:
                //            v =>
                //                mgr.CreateShortcutsForExecutable(exePath, shortcutPaths, false),
                //        onAppUpdate:
                //            v =>
                //                mgr.CreateShortcutsForExecutable(exePath, shortcutPaths, true),
                //        onAppUninstall:
                //            v =>
                //                mgr.RemoveShortcutsForExecutable(exePath, shortcutPaths));
                //}

                Task.Run(async () => await StartupAsync());
            }
            catch
            {
                // ignored
            }
        }

        private async Task StartupAsync()
        {
            using (var mgr = new UpdateManager(UpdateUrl))
            {
                await mgr.UpdateApp();
            }
        }
    }
}