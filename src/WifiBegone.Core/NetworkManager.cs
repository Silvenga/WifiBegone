namespace WifiBegone.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    using SimpleWifi;

    using WifiBegone.Core.Models;

    public interface INetworkManager
    {
        Task<NetworkSurvey> GetNetworkSurvey();
        Task<bool> HasInternetAccess(string interfaceId);
        void DisconnectWifi();
        void ConnectWifi();
    }

    public class NetworkManager : INetworkManager
    {
        public async Task<NetworkSurvey> GetNetworkSurvey()
        {
            var internetInterfaceTypes = new[]
            {
                NetworkInterfaceType.Wireless80211,
                NetworkInterfaceType.Ethernet,
                NetworkInterfaceType.GigabitEthernet,
                NetworkInterfaceType.FastEthernetFx,
                NetworkInterfaceType.Ethernet3Megabit,
                NetworkInterfaceType.FastEthernetT,
            };

            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            var activeInterfaces = networkInterfaces
                .Where(x => x.OperationalStatus == OperationalStatus.Up)
                .Where(x => internetInterfaceTypes.Contains(x.NetworkInterfaceType));

            var accessableInterfaces = new List<NetworkInterface>();
            foreach (var networkInterface in activeInterfaces)
            {
                var result = await HasInternetAccess(networkInterface.Id);
                if (result)
                {
                    accessableInterfaces.Add(networkInterface);
                }
            }
            var groups = accessableInterfaces
                .GroupBy(x => x.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                .ToLookup(x => x.Key)
                .ToDictionary(x => x.Key, x => x.SelectMany(c => c).ToList());

            return new NetworkSurvey
            {
                WiredNetworks = groups.ContainsKey(false) ? groups[false] : new List<NetworkInterface>(),
                WifiNetworks = groups.ContainsKey(true) ? groups[true] : new List<NetworkInterface>()
            };
        }

        public async Task<bool> HasInternetAccess(string interfaceId)
        {
            var networkInterface = NetworkInterface
                .GetAllNetworkInterfaces()
                .SingleOrDefault(x => x.Id == interfaceId);

            if (networkInterface == null)
            {
                throw new InvalidOperationException($"Interface does not exist: {interfaceId}");
            }

            var addresses = networkInterface
                .GetIPProperties()
                .UnicastAddresses;

            foreach (var address in addresses)
            {
                try
                {
                    var localEndPoint = new IPEndPoint(address.Address, 0);
                    var tcpClient = new TcpClient(localEndPoint);

                    await tcpClient.ConnectAsync("google.com", 80);

                    if (tcpClient.Connected)
                    {
                        return true;
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            return false;
        }

        public void DisconnectWifi()
        {
            var wifi = new Wifi();
            wifi.Disconnect();
        }

        public void ConnectWifi()
        {
            var wifi = new Wifi();
            if (wifi.NoWifiAvailable)
            {
                return;
            }

            var bestAccessPoint = wifi.GetAccessPoints()
                                      .Where(x => x.HasProfile)
                                      .OrderByDescending(x => x.SignalStrength);

            foreach (var accessPoint in bestAccessPoint)
            {
                var auth = new AuthRequest(accessPoint);
                accessPoint.Connect(auth);
            }
        }
    }
}