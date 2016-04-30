using System.Windows;

namespace WifiBegone.Tray
{
    using System.Threading.Tasks;

    using WifiBegone.Tray.Service;

    public partial class App : Application
    {
        public App()
        {
            SquirrelManager.StartupEventsAsync().Wait();

            Task.Run(async () => await SquirrelManager.UpdateAsync());
        }
    }
}