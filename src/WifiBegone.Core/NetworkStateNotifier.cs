namespace WifiBegone.Core
{
    using System;
    using System.Net.NetworkInformation;
    using System.Threading.Tasks;

    using WifiBegone.Core.Models;

    public interface INetworkStateNotifier
    {
        bool Running { get; set; }
        event Events.NetworkStateChanged NetworkStateChanged;
        void Start();
    }

    public class NetworkStateNotifier : INetworkStateNotifier
    {
        private readonly INetworkManager _manager;
        public NetworkSurvey LastSurvey { get; set; }

        public bool Running { get; set; }

        public event Events.NetworkStateChanged NetworkStateChanged;

        public NetworkStateNotifier(INetworkManager manager)
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

            NetworkChange.NetworkAddressChanged += (sender, args) => PollStateChanges();
            NetworkChange.NetworkAvailabilityChanged += (sender, args) => PollStateChanges();

            Running = true;
            PollStateChanges();
        }

        public void PollStateChanges()
        {
            var nextSurvey = _manager.GetNetworkSurvey();
            var nextState = nextSurvey.NetworkState;
            Console.Write(".");

            if (nextState != LastSurvey?.NetworkState)
            {
                Console.WriteLine();
                Console.WriteLine(nextSurvey);
                OnNetworkStateChanged(this, new NetworkStateChangedArgs(LastSurvey, nextSurvey));
                LastSurvey = nextSurvey;
            }
        }

        protected virtual void OnNetworkStateChanged(object source, NetworkStateChangedArgs e)
        {
            NetworkStateChanged?.Invoke(source, e);
        }
    }
}