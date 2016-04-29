using System.Windows;

namespace WifiBegone.Tray
{
    using System.Threading.Tasks;

    using Squirrel;

    public partial class App : Application
    {
        private const string UpdateUrl = "https://github.com/Silvenga/WifiBegone";

        public App()
        {
            using (var mgr = UpdateManager.GitHubUpdateManager(UpdateUrl).Result)
            {
                const string exePath = "WifiBegone.Tray.exe";
                const ShortcutLocation shortcutPaths = ShortcutLocation.StartMenu | ShortcutLocation.Startup;

                SquirrelAwareApp.HandleEvents(
                    onInitialInstall:
                        v =>
                            mgr.CreateShortcutsForExecutable(exePath, shortcutPaths, false),
                    onAppUpdate:
                        v =>
                            mgr.CreateShortcutsForExecutable(exePath, shortcutPaths, true),
                    onAppUninstall:
                        v =>
                            mgr.RemoveShortcutsForExecutable(exePath, shortcutPaths));
            }

            Task.Run(async () => await UpdateLoopAsync());
        }

        private async Task UpdateLoopAsync()
        {
            using (var mgr = UpdateManager.GitHubUpdateManager(UpdateUrl).Result)
            {
                await mgr.UpdateApp();
            }
        }
    }
}