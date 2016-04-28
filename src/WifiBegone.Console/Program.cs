namespace WifiBegone.Console
{
    using System;

    using WifiBegone.Core;

    public static class Program
    {
        private static void Main(string[] args)
        {
            var manager = new NetworkManager();
            var notifier = new NetworkStateNotifier(manager);
            var reactor = new NetworkStateReactor(manager, notifier, new ConsoleLogger());
            notifier.Start();

            Console.ReadLine();
        }
    }
}