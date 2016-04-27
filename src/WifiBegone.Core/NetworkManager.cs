namespace WifiBegone.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Net.Sockets;

    using SimpleWifi;

    using WifiBegone.Core.Models;

    public interface INetworkManager
    {
        NetworkSurvey GetNetworkSurvey();
        bool HasInternetAccess(NetworkInterface networkInterface);
        void DisconnectWifi();
        void ConnectWifi();
    }

    public class NetworkManager : INetworkManager
    {
        public NetworkSurvey GetNetworkSurvey()
        {
            var internetInterfaceTypes = new HashSet<NetworkInterfaceType>
            {
                NetworkInterfaceType.Wireless80211,
                NetworkInterfaceType.Ethernet,
                NetworkInterfaceType.GigabitEthernet,
                NetworkInterfaceType.FastEthernetFx,
                NetworkInterfaceType.Ethernet3Megabit,
                NetworkInterfaceType.FastEthernetT,
            };

            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            var accessableInterfaces = networkInterfaces
                .Where(x => x.OperationalStatus == OperationalStatus.Up)
                .Where(x => internetInterfaceTypes.Contains(x.NetworkInterfaceType))
                .AsParallel()
                .Where(HasInternetAccess)
                .Select(x => new
                {
                    IsWifi = x.NetworkInterfaceType == NetworkInterfaceType.Wireless80211
                })
                .ToList();

            return new NetworkSurvey
            {
                WiredNetworks = accessableInterfaces.Count(x => !x.IsWifi),
                WifiNetworks = accessableInterfaces.Count(x => x.IsWifi)
            };
        }

        public bool HasInternetAccess(NetworkInterface networkInterface)
        {
            var addresses = networkInterface
                .GetIPProperties()
                .UnicastAddresses
                .Select(x => x.Address)
                .Where(x => x.AddressFamily == AddressFamily.InterNetwork || x.IsIPv6Multicast);

            foreach (var address in addresses)
            {
                try
                {
                    var localEndPoint = new IPEndPoint(address, 0);
                    var tcpClient = new TcpClient(localEndPoint);

                    tcpClient.Connect("8.8.8.8", 53);

                    if (tcpClient.Connected)
                    {
                        return true;
                    }
                }
                catch
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
                var success = accessPoint.Connect(auth);
                if (success)
                {
                    return;
                }
            }
        }
    }
}