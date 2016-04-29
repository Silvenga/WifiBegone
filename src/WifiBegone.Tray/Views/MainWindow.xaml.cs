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

            _tray = new TrayIcon();
            _tray.Show();

            var trayLogger = new TrayLogger(_tray);
            _backgroundService = new BackgroundService(trayLogger);

            _backgroundService.Start();
        }
    }
}