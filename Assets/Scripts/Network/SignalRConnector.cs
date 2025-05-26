using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;

public class SignalRConnector : SignalConnectorBase
{
    private HubConnection connection;

    public override async Task<HubConnection> GetConnection()
    {
        if (connection != null)
        {
            return connection;
        }
        
        connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5080/chatHub", options =>
            {
                
            }) // URL to the SignalR hub
            .Build();
      
        try
        {
            await connection.StartAsync();
            Debug.Log("SignalR Connected");
        }
        catch (Exception e)
        {
            Debug.LogError($"Connection failed: {e.Message}");
        }
        return connection;
    }

    public override void SendShips(int fromId, int toId, int amount)
    {
        connection.InvokeAsync("SendShips", fromId, toId, amount);
    }

    public override void OnPlayerJoin(Action<string> callback)
    {
        
    }

    public override void OnPlayerLeft(Action<int, string> callback)
    {
    }

    public override void OnLoadGame(Action<bool> callback)
    {
    }

    public override void OnGameStarted(Action<bool> callback)
    {
    }

    public override void OnSendShips(Action<int, int, int> callback)
    {
    }

    public override void RemoveGameLobbyMethods()
    {
    }

    public override void RemoveGameManagerMethods()
    {
    }

    public override void StartGame()
    {
    }

    public override void LeaveGame()
    {
        throw new NotImplementedException();
    }

    public override void Ready(int userId, string roomId)
    {
    }
}
