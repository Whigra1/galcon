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
        Debug.Log($"Sending ships from {fromId} to {toId} with amount {amount}");
        connection.InvokeAsync("SendShips", fromId, toId, amount);
    }
}
