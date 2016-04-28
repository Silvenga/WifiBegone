using System;

namespace WifiBegone.Tray.Service
{
    using WifiBegone.Core;
    using WifiBegone.Tray.Views;

    public class TrayLogger : ILogger
    {
        private readonly TrayIcon _icon;

        public TrayLogger(TrayIcon icon)
        {
            _icon = icon;
        }

        public void Info(string message)
        {
            Console.WriteLine(message);
        }

        public void Notify(string message)
        {
            _icon.Notification(message);
        }
    }
}