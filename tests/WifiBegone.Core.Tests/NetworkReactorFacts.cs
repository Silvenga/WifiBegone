namespace WifiBegone.Core.Tests
{
    using System;
    using System.Linq;

    using NSubstitute;

    using WifiBegone.Core.Models;

    using Xunit;

    public class NetworkReactorFacts
    {
        private readonly INetworkManager _manager = Substitute.For<INetworkManager>();
        private readonly NetworkReactor _reactor;

        public NetworkReactorFacts()
        {
            _reactor = new NetworkReactor(_manager);
        }

        [Theory,
         InlineData(NetworkState.Unknown, NetworkState.Both),
         InlineData(NetworkState.NoNetwork, NetworkState.Both),
         InlineData(NetworkState.OnlyWifi, NetworkState.Both),
         InlineData(NetworkState.OnlyWired, NetworkState.Both)]
        public void When_state_moves_to_both_disable_wifi(NetworkState from, NetworkState to)
        {
            // Act
            _reactor.OnStateChange(from, to);

            // Assert
            _manager.Received().DisconnectWifi();
        }
    }
}