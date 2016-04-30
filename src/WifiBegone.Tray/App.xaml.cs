using System.Windows;

namespace WifiBegone.Tray
{
    using System.Threading.Tasks;

    public partial class App : Application
    {
        public App()
        {
            SquirrelManager.StartupEventsAsync().Wait();

            Task.Run(async () => await SquirrelManager.UpdateAsync());
        }
    }
}