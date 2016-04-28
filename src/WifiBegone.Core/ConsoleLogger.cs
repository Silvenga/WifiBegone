namespace WifiBegone.Core
{
    using System;

    public interface ILogger
    {
        void Info(string message);
        void Notify(string message);
    }

    public class ConsoleLogger : ILogger
    {
        public void Info(string message)
        {
            Console.WriteLine(message);
        }

        public void Notify(string message)
        {
            Console.WriteLine(message);
        }
    }
}