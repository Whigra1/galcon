using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;

namespace Network.Signal
{
    public abstract class GameConnectorBase : MonoBehaviour, ISignalConnector
    {
        public abstract Task<HubConnection> StartConnection();
        public abstract Task CloseConnection();

        public abstract void StartGame();
        public abstract void SendShips(string fromId, string toId, float portion);
     
        // Events
        public abstract void OnGameStarted(Action<object> callback);
        public abstract void OnSendShips(Action<object> callback);
        public abstract void OnShipsArrived(Action<object> callback);
        public abstract void OnPlanetProducedShips (Action<object> callback);
        
        public abstract void OnGameEnded(Action<object> callback);
    }
}