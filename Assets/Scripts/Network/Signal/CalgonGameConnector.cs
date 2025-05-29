using System;
using System.Threading.Tasks;
using EventArgs;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;

namespace Network.Signal
{
    public class CalgonGameConnector : GameConnectorBase
    {
        private static HubConnection _connection;
        public override async Task<HubConnection> StartConnection()
        {
            if (_connection != null)
            {
                return _connection;
            }
        
            _connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/api/hubs/game?roomId=" + RoomInfo.Id, options =>
                {
                    options.Headers.Add("Authorization", "Bearer " + UserData.Token);
                })
                .Build();
      
            try
            {
                await _connection.StartAsync();
                Debug.Log("SignalR Connected");
            }
            catch (Exception e)
            {
                Debug.LogError($"Connection failed: {e.Message}");
            }
            return _connection;
        }

        public override async Task CloseConnection()
        {
            if (_connection is not null)
            {
                await _connection.StopAsync();
                await _connection.DisposeAsync();
            }
            _connection = null;
        }

        public override async void StartGame()
        {
            var connection = await StartConnection();
            await connection.InvokeAsync("StartGame");
        }

        public override void SendShips(string fromId, string toId, float portion)
        {
            _connection.InvokeAsync("SendFleet", new SendFleetArgs
            {
                DeparturePlanetId = Guid.Parse(fromId),
                DestinationPlanetId = Guid.Parse(toId),
                Portion = portion
            });
        }

        public override void OnGameStarted(Action<object> callback)
        {
            _connection.On("GameStarted", callback);
        }

        public override void OnSendShips(Action<object> callback)
        {
            _connection.On("FleetSent", callback);
        }

        public override void OnShipsArrived(Action<object> callback)
        {
            _connection.On("FleetArrived", callback);
        }

        public override void OnPlanetProducedShips(Action<object> callback)
        {
            _connection.On("ShipsProduced", callback);
        }

        public override void OnGameEnded(Action<object> callback)
        {
            _connection.On("GameEnded", callback);
        }
    }
}