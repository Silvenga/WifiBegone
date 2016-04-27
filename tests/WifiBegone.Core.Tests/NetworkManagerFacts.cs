namespace WifiBegone.Core.Tests
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Xunit;
    using Xunit.Abstractions;

    public class NetworkManagerFacts
    {
        private readonly ITestOutputHelper _helper;

        public NetworkManagerFacts(ITestOutputHelper helper)
        {
            _helper = helper;
        }

        [Fact]
        public async Task Test()
        {
            // Act
            var manager = new NetworkManager();

            var avg = 0L;

            var stopwatch = new Stopwatch();

            var rounds = 10;

            for (var i = 0; i < rounds; i++)
            {
                stopwatch.Reset();
                stopwatch.Start();
                manager.GetNetworkSurvey();
                stopwatch.Stop();
                avg += stopwatch.ElapsedMilliseconds;
            }

            // Assert
            _helper.WriteLine(avg / rounds + "ms");
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