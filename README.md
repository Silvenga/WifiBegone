![WifiBegone](assets/icon.png)

# WifiBegone

[![AppVeyor](https://img.shields.io/appveyor/ci/Silvenga/wifibegone.svg?maxAge=2592000&style=flat-square)](https://ci.appveyor.com/project/Silvenga/wifibegone)

Sure, there's ways to prioritize connections, but they don't always work (think HyperV virtual switches). WifiBegone disconnects from WiFi when wired in, reconnects when not - simple!

[Download Latest Release](https://github.com/Silvenga/WifiBegone/releases/)

## How does it work?

When the program detects a wireless and wired active connection, it will attempt to disconnect from the wireless using the native WiFi interface found in Windows. 

To detect active interfaces this program:

- Finds all wireless and wired "Up" interfaces installed. VPN and tunnels are not considered. 
- For each interface, bind a TCP client.
- Attempt to connect to 8.8.8.8 (Google's public DNS server) on port 53. 
- If success, mark connection as active

When a network change from wired to no network is detected, it will attempt to connect to the strongest WiFi connection that has a current wireless profile. 

## TODO

- [X] Handle IPv6 only connections
- [ ] Testing
- [X] Installer
- [X] Custom Icon
- [ ] Detect network changes, don't poll