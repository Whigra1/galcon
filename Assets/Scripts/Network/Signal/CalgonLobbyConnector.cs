using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace Network.Signal
{
    public class CalgonLobbyConnector : LobbyConnectorBase
    {
        private HubConnection _connection;
        public override async Task<HubConnection> StartConnection()
        {
            if (_connection != null) { return _connection; }
            _connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/api/hubs/room?roomId=" + RoomInfo.Id, options =>
                {
                    options.Headers.Add("Authorization", "Bearer " + UserData.Token);
                })
                .Build();
            await _connection.StartAsync();
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

        public override async Task OnRoomUpdate(Action<object> obj)
        {
            var connection = await StartConnection();
            connection.On("room_update", obj);
        }
    }
}