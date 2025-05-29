using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace Network.Signal
{
    public interface ISignalConnector
    {
        public Task<HubConnection> StartConnection();
        public Task CloseConnection();
    }
}