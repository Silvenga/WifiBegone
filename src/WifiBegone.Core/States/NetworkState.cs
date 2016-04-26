namespace WifiBegone.Core.States
{
    public enum NetworkState
    {
        Unknown,
        NoNetwork,
        OnlyWifi,
        OnlyWired,
        Both
    }

    public class StateCollection
    {
        private readonly NetworkState _from;
        private readonly NetworkState _to;

        public StateCollection(NetworkState from, NetworkState to)
        {
            _from = from;
            _to = to;
        }

        public bool To(NetworkState from, NetworkState to)
        {
            return _from == from && _to == to;
        }
    }
}