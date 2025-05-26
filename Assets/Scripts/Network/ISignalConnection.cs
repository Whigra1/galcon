using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;

public abstract class SignalConnectorBase : MonoBehaviour
{
    public abstract Task<HubConnection> GetConnection();
    
    // Events
    
    public abstract void SendShips(int fromId, int toId, int amount);
    public abstract void OnPlayerJoin(Action<string> callback);
    public abstract void OnPlayerLeft(Action<int, string> callback);
    public abstract void OnLoadGame(Action<bool> callback);
    public abstract void OnGameStarted(Action<bool> callback);
    
    public abstract void OnSendShips(Action<int, int, int> callback);
    
    // Utility methods
    public abstract void RemoveGameLobbyMethods();
    public abstract void RemoveGameManagerMethods();
    
    // Commands
    public abstract void StartGame();
    public abstract void LeaveGame();
    public abstract void Ready(int userId, string roomId);
}
