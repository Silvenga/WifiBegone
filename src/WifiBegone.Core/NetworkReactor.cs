namespace WifiBegone.Core
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using WifiBegone.Core.Models;

    public class NetworkReactor
    {
        private readonly INetworkManager _manager;
        public NetworkSurvey LastSurvey { get; set; }
        public NetworkState LastState { get; set; }

        public bool Running { get; set; }

        public NetworkReactor(INetworkManager manager)
        {
            _manager = manager;
        }

        public void Start()
        {
            Task.Run(() => ReactorLoopAsync());
        }

        private void ReactorLoopAsync()
        {
            Console.WriteLine("Starting loop.");
            Running = true;
            while (Running)
            {
                GetStateChanges();
                Thread.Sleep(1000 * 1);
            }
        }

        public void GetStateChanges()
        {
            var nextSurvey = _manager.GetNetworkSurvey();
            var nextState = nextSurvey.NetworkState;
            Console.Write(".");

            if (nextState != LastState)
            {
                Console.WriteLine();
                Console.WriteLine(nextSurvey);
                OnStateChange(LastState, nextState);
                LastSurvey = nextSurvey;
                LastState = nextState;
            }
        }

        public void OnStateChange(NetworkState currentState, NetworkState nextState)
        {
            Console.WriteLine($"{currentState} -> {nextState}");

            switch (currentState)
            {
                case NetworkState.Unknown:
                    FromUnknown(nextState);
                    break;
                case NetworkState.NoNetwork:
                    FromNoNetwork(nextState);
                    break;
                case NetworkState.OnlyWifi:
                    FromOnlyWifi(nextState);
                    break;
                case NetworkState.OnlyWired:
                    FromOnlyWired(nextState);
                    break;
                case NetworkState.Both:
                    FromBoth(nextState);
                    break;
            }
        }

        private void FromUnknown(NetworkState nextState)
        {
            switch (nextState)
            {
                case NetworkState.OnlyWifi:
                    Console.WriteLine("No action.");
                    break;
                case NetworkState.OnlyWired:
                    Console.WriteLine("No action.");
                    break;
                case NetworkState.Both:
                    Console.WriteLine("Disconnect Wifi.");
                    _manager.DisconnectWifi();
                    break;
            }
        }

        private void FromNoNetwork(NetworkState nextState)
        {
            switch (nextState)
            {
                case NetworkState.OnlyWifi:
                    Console.WriteLine("No action.");
                    break;
                case NetworkState.OnlyWired:
                    Console.WriteLine("No action.");
                    break;
                case NetworkState.Both:
                    Console.WriteLine("Disconnect Wifi.");
                    _manager.DisconnectWifi();
                    break;
            }
        }

        private void FromOnlyWifi(NetworkState nextState)
        {
            switch (nextState)
            {
                case NetworkState.NoNetwork:
                    Console.WriteLine("No action.");
                    break;
                case NetworkState.OnlyWired:
                    Console.WriteLine("No action.");
                    break;
                case NetworkState.Both:
                    Console.WriteLine("Disconnect Wifi.");
                    _manager.DisconnectWifi();
                    break;
            }
        }

        private void FromOnlyWired(NetworkState nextState)
        {
            switch (nextState)
            {
                case NetworkState.NoNetwork:
                    Console.WriteLine("No action.");
                    break;
                case NetworkState.OnlyWifi:
                    Console.WriteLine("No action.");
                    break;
                case NetworkState.Both:
                    Console.WriteLine("Disconnect Wifi.");
                    _manager.DisconnectWifi();
                    break;
            }
        }

        private void FromBoth(NetworkState nextState)
        {
            switch (nextState)
            {
                case NetworkState.NoNetwork:
                    Console.WriteLine("No action.");
                    break;
                case NetworkState.OnlyWifi:
                    Console.WriteLine("No action.");
                    break;
                case NetworkState.OnlyWired:
                    Console.WriteLine("No action.");
                    break;
            }
        }
    }
}