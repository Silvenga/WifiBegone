namespace WifiBegone.Core.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.NetworkInformation;

    using WifiBegone.Core.States;

    public class NetworkSurvey
    {
        public IList<NetworkInterface> WiredNetworks { get; set; }

        public IList<NetworkInterface> WifiNetworks { get; set; }

        public bool HasWiredNetworks => WiredNetworks?.Any() == true;

        public bool HasWifiNetworks => WifiNetworks?.Any() == true;

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
                $"Wired: {WiredNetworks.Count}, Wifi: {WifiNetworks.Count}, HasWired: {HasWiredNetworks}, HasWifi: {HasWifiNetworks}, State: {NetworkState}";
        }
    }
}