namespace WifiBegone.Console
{
    using System;

    using WifiBegone.Core;

    public static class Program
    {
        private static void Main(string[] args)
        {
            var manager = new NetworkManager();
            var reactor = new NetworkReactor(manager);
            reactor.Start();

            Console.ReadLine();
        }
    }
}