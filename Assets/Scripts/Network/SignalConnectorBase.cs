using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;

public abstract class SignalConnectorBase : MonoBehaviour
{
    public abstract Task<HubConnection> GetConnection();
    
    // Events
    public abstract void OnGameStarted(Action<object> callback);
    public abstract void OnRoomUpdate(Action<object> callback);
    public abstract void OnSendShips(Action<object> callback);
    public abstract void OnPlanetProducedShips (Action<object> callback);
    
    // Utility methods
    public abstract void RemoveGameLobbyMethods();
    public abstract void RemoveGameManagerMethods();
    public abstract Task CloseConnection();
    
    // Commands
    public abstract void StartGame();
    public abstract void SendShips(string fromId, string toId, float portion);

}
