using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;

namespace Network.Signal
{
    public abstract class LobbyConnectorBase: MonoBehaviour, ISignalConnector
    {
        private HubConnection _connection;
        public abstract Task<HubConnection> StartConnection();
        public abstract Task CloseConnection();

        public abstract Task OnRoomUpdate(Action<object> obj);
    }
}