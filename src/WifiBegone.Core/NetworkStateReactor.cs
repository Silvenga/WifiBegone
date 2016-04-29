namespace WifiBegone.Core
{
    using WifiBegone.Core.Models;

    public class NetworkStateReactor
    {
        private readonly INetworkManager _manager;
        private readonly INetworkStateNotifier _stateNotifier;
        private readonly ILogger _logger;

        public NetworkStateReactor(INetworkManager manager, INetworkStateNotifier stateNotifier, ILogger logger)
        {
            _manager = manager;
            _stateNotifier = stateNotifier;
            _logger = logger;

            _stateNotifier.NetworkStateChanged += StateNotifierOnNetworkStateChanged;
        }

        private void StateNotifierOnNetworkStateChanged(object source, NetworkStateChangedArgs networkStateChangedArgs)
        {
            OnStateChange(
                networkStateChangedArgs.LastNetworkSurvey?.NetworkState ?? NetworkState.Unknown,
                networkStateChangedArgs.NextNetworkSurvey.NetworkState
                );
        }

        public void OnStateChange(NetworkState currentState, NetworkState nextState)
        {
            _logger.Info($"{currentState} -> {nextState}");

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
                case NetworkState.NoNetwork:
                    _logger.Notify($"Wired disconnected, trying wireless. ({NetworkState.Unknown} -> {nextState})");
                    _manager.ConnectWifi();
                    break;
                case NetworkState.OnlyWifi:
                    _logger.Info("No action.");
                    break;
                case NetworkState.OnlyWired:
                    _logger.Info("No action.");
                    break;
                case NetworkState.Both:
                    _logger.Notify(
                        $"Detected active wired connection, disconnecting wireless. ({NetworkState.Unknown} -> {nextState})");
                    _manager.DisconnectWifi();
                    break;
            }
        }

        private void FromNoNetwork(NetworkState nextState)
        {
            switch (nextState)
            {
                case NetworkState.OnlyWifi:
                    _logger.Info("No action.");
                    break;
                case NetworkState.OnlyWired:
                    _logger.Info("No action.");
                    break;
                case NetworkState.Both:
                    _logger.Notify(
                        $"Detected active wired connection, disconnecting wireless. ({NetworkState.NoNetwork} -> {nextState})");
                    _manager.DisconnectWifi();
                    break;
            }
        }

        private void FromOnlyWifi(NetworkState nextState)
        {
            switch (nextState)
            {
                case NetworkState.NoNetwork:
                    _logger.Info("No action.");
                    break;
                case NetworkState.OnlyWired:
                    _logger.Info("No action.");
                    break;
                case NetworkState.Both:
                    _logger.Notify(
                        $"Detected active wired connection, disconnecting wireless. ({NetworkState.OnlyWifi} -> {nextState})");
                    _manager.DisconnectWifi();
                    break;
            }
        }

        private void FromOnlyWired(NetworkState nextState)
        {
            switch (nextState)
            {
                case NetworkState.NoNetwork:
                    _logger.Notify($"Wired disconnected, trying wireless. ({NetworkState.OnlyWired} -> {nextState})");
                    _manager.ConnectWifi();
                    break;
                case NetworkState.OnlyWifi:
                    _logger.Info("No action.");
                    break;
                case NetworkState.Both:
                    _logger.Notify(
                        $"Detected active wired connection, disconnecting wireless. ({NetworkState.OnlyWired} -> {nextState})");
                    _manager.DisconnectWifi();
                    break;
            }
        }

        private void FromBoth(NetworkState nextState)
        {
            switch (nextState)
            {
                case NetworkState.NoNetwork:
                    _logger.Info("No action.");
                    break;
                case NetworkState.OnlyWifi:
                    _logger.Info("No action.");
                    break;
                case NetworkState.OnlyWired:
                    _logger.Info("No action.");
                    break;
            }
        }
    }
}