namespace WifiBegone.Tray.Service
{
    using WifiBegone.Core;

    public class BackgroundService
    {
        private readonly ILogger _logger;
        private readonly NetworkManager _manager;
        private readonly NetworkStateNotifier _stateNotifier;
        private readonly NetworkStateReactor _reactor;

        public BackgroundService(ILogger logger)
        {
            _logger = logger;
            _manager = new NetworkManager();
            _stateNotifier = new NetworkStateNotifier(_manager);
            _reactor = new NetworkStateReactor(_manager, _stateNotifier, _logger);
        }

        public void Start()
        {
            _stateNotifier.Start();
        }
    }
}