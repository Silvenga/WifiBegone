namespace WifiBegone.Core.Models
{
    using System;

    public class Events
    {
        public delegate void NetworkStateChanged(object source, NetworkStateChangedArgs e);
    }

    public class NetworkStateChangedArgs : EventArgs
    {
        public NetworkSurvey LastNetworkSurvey { get; set; }
        public NetworkSurvey NextNetworkSurvey { get; set; }

        public NetworkStateChangedArgs(NetworkSurvey lastNetworkSurvey, NetworkSurvey nextNetworkSurvey)
        {
            LastNetworkSurvey = lastNetworkSurvey;
            NextNetworkSurvey = nextNetworkSurvey;
        }
    }
}