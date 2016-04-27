namespace WifiBegone.Core.Models
{
    public class NetworkSurvey
    {
        public int WiredNetworks { get; set; }

        public int WifiNetworks { get; set; }

        public bool HasWiredNetworks => WiredNetworks > 0;

        public bool HasWifiNetworks => WifiNetworks > 0;

        public NetworkState NetworkState => CreateState();

        private NetworkState CreateState()
        {
            if (HasWiredNetworks && HasWifiNetworks)
            {
                return NetworkState.Both;
            }

            if (HasWifiNetworks)
            {
                return NetworkState.OnlyWifi;
            }

            if (HasWiredNetworks)
            {
                return NetworkState.OnlyWired;
            }

            return NetworkState.NoNetwork;
        }

        public override string ToString()
        {
            return
                $"Wired: {WiredNetworks}, Wifi: {WifiNetworks}, HasWired: {HasWiredNetworks}, HasWifi: {HasWifiNetworks}, State: {NetworkState}";
        }
    }
}