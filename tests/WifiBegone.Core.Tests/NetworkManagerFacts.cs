namespace WifiBegone.Core.Tests
{
    using System.Threading.Tasks;

    using Xunit;

    public class NetworkManagerFacts
    {
        [Fact]
        public async Task Test()
        {
            // Act
            var manager = new NetworkManager();

            // Assert
            var survey = await manager.GetNetworkSurvey();
        }

        [Fact(Skip = "Bad")]
        public async Task Test2()
        {
            // Act
            var manager = new NetworkManager();

            // Assert
            manager.DisconnectWifi();
        }
    }
}