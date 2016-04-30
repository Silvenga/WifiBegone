namespace WifiBegone.Tray.Service
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Squirrel;

    public static class SquirrelManager
    {
        private const string UpdateUrl = "https://github.com/Silvenga/WifiBegone";
        private const ShortcutLocation ShortcutPaths = ShortcutLocation.StartMenu | ShortcutLocation.Startup;

        private static string ExePath => Path.GetFileName(Assembly.GetEntryAssembly().Location);

        private static async Task<UpdateManager> CreateUpdateManagerAsync()
        {
            var manager = await UpdateManager.GitHubUpdateManager(UpdateUrl);
            return manager;
        }

        public static async Task StartupEventsAsync()
        {
            using (var mgr = await CreateUpdateManagerAsync())
            {
                SquirrelAwareApp.HandleEvents(
                    onInitialInstall:
                        v =>
                            mgr.CreateShortcutsForExecutable(ExePath, ShortcutPaths, false),
                    onAppUpdate:
                        v =>
                            mgr.CreateShortcutsForExecutable(ExePath, ShortcutPaths, true),
                    onAppUninstall:
                        v =>
                            mgr.RemoveShortcutsForExecutable(ExePath, ShortcutPaths));
            }
        }

        public static async Task UpdateAsync()
        {
            using (var mgr = await CreateUpdateManagerAsync())
            {
                var updates = await mgr.CheckForUpdate();
                if (updates.ReleasesToApply.Any())
                {
                    var release = updates.ReleasesToApply.OrderByDescending(x => x.Version).First();
                    await ApplyUpdateAsync(release.Version.Version, mgr);
                }
            }
        }

        private static async Task ApplyUpdateAsync(Version lastVersion, UpdateManager mgr)
        {
            await mgr.UpdateApp();

            var folder = $"app-{lastVersion.Major}.{lastVersion.Minor}.{lastVersion.Build}";
            var latestExe = Path.Combine(mgr.RootAppDirectory, folder, ExePath);

            UpdateManager.RestartApp(latestExe);
        }
    }
}