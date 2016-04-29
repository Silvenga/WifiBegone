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
            Install();
        }

        private void Install()
        {
            using (var mgr = UpdateManager.GitHubUpdateManager(UpdateUrl))
            {
                const string exePath = "WifiBegone.Tray.exe";
                const ShortcutLocation shortcutPaths = ShortcutLocation.StartMenu | ShortcutLocation.Startup;

                SquirrelAwareApp.HandleEvents(
                    onInitialInstall:
                        v =>
                            mgr.Result.CreateShortcutsForExecutable(exePath, shortcutPaths, false),
                    onAppUpdate:
                        v =>
                            mgr.Result.CreateShortcutsForExecutable(exePath, shortcutPaths, true),
                    onAppUninstall:
                        v =>
                            mgr.Result.RemoveShortcutsForExecutable(exePath, shortcutPaths));
            }

            Task.Run(async () => await UpdateLoopAsync());
        }

        private async Task UpdateLoopAsync()
        {
            using (var mgr = UpdateManager.GitHubUpdateManager(UpdateUrl))
            {
                await mgr.Result.UpdateApp();
            }
        }
    }
}