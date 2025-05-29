using System;
using System.Threading.Tasks;
using EventArgs;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class SignalRConnector : SignalConnectorBase
{
    private static HubConnection connection;

    public override async Task<HubConnection> GetConnection()
    {
        if (connection != null)
        {
            return connection;
        }
        
        connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5000/api/hubs/game?roomId=" + RoomInfo.Id, options =>
            {
                options.Headers.Add("Authorization", "Bearer " + UserData.Token);
            })
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

    public override void SendShips(string fromId, string toId, float portion)
    {
        connection.InvokeAsync("SendFleet", new SendFleetArgs
        {
            DeparturePlanetId = Guid.Parse(fromId),
            DestinationPlanetId = Guid.Parse(toId),
            Portion = portion
        });
    }
    
    public override async void OnGameStarted(Action<object> callback)
    {
        connection.On("GameStarted", callback);
    }

    public override void OnRoomUpdate(Action<object> callback)
    {
        connection.On("room_update", callback);
    }

    public override void OnSendShips(Action<object> callback)
    {
        connection.On("FleetSent", callback);
    }

    public override void OnPlanetProducedShips(Action<object> callback)
    {
        connection.On("ShipsProduced", callback);
    }

    public override void RemoveGameLobbyMethods()
    {
    }

    public override void RemoveGameManagerMethods()
    {
    }

    public override async void StartGame()
    {
        var connection = await GetConnection();
        await connection.InvokeAsync("StartGame");
    }

    public override async Task CloseConnection()
    {
        await connection.StopAsync();
        connection = null;
    }
}
