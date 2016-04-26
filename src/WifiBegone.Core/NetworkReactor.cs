namespace WifiBegone.Core
{
    using System;
    using System.Threading.Tasks;

    using WifiBegone.Core.Models;
    using WifiBegone.Core.States;

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
            Task.Run(async () => await ReactorLoop());
        }

        private async Task ReactorLoop()
        {
            Console.WriteLine("Starting loop.");
            Running = true;
            while (Running)
            {
                await GetStateChanges();
                await Task.Delay(1000 * 1);
            }
        }

        public async Task GetStateChanges()
        {
            var nextSurvey = await _manager.GetNetworkSurvey();
            var nextState = nextSurvey.NetworkState;
            Console.Write(".");

            if (nextState != LastState)
            {
                Console.WriteLine();
                Console.WriteLine(nextSurvey);
                await OnStateChange(LastState, nextState);
                LastSurvey = nextSurvey;
                LastState = nextState;
            }
        }

        public async Task OnStateChange(NetworkState currentState, NetworkState nextState)
        {
            Console.WriteLine($"{currentState} -> {nextState}");

            switch (currentState)
            {
                case NetworkState.NoNetwork:
                    await FromNoNetwork(nextState);
                    break;
                case NetworkState.OnlyWifi:
                    await FromOnlyWifi(nextState);
                    break;
                case NetworkState.OnlyWired:
                    await FromOnlyWired(nextState);
                    break;
                case NetworkState.Both:
                    await FromBoth(nextState);
                    break;
            }
        }

        private async Task FromNoNetwork(NetworkState nextState)
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

        private async Task FromOnlyWifi(NetworkState nextState)
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

        private async Task FromOnlyWired(NetworkState nextState)
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

        private async Task FromBoth(NetworkState nextState)
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