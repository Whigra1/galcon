using System.Collections.Generic;
using System.Threading.Tasks;
using Network;
using UnityEngine;
using Microsoft.AspNetCore.SignalR.Client;
public class MockHttpProvider : GameHttpProviderBase, IGameHttpApiProvider
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public SignalConnectorBase signalConnector;
    
    public override async Task<ApiRequestResult<UserInfoDto>> Register(string login, string password)
    {
        var taskCompletionSource = new TaskCompletionSource<ApiRequestResult<UserInfoDto>>();
        var connection = await signalConnector.GetConnection();
        connection.On<string, int>("Registered", (token, id) =>
        {
            connection.Remove("Registered");
            taskCompletionSource.SetResult(new ApiRequestResult<UserInfoDto>
            {
                Data = new UserInfoDto
                {
                    Id = id,
                    Token = token,
                    Name = login,
                },
                ErrorMessage = id < 0 ? "Failed to register" : "",
            });
        });
        
        await connection.InvokeAsync("Register", login, password);
        
        return await taskCompletionSource.Task;
    }

    public override async Task<ApiRequestResult<UserInfoDto>> Login(string login, string password)
    {
        var taskCompletionSource = new TaskCompletionSource<ApiRequestResult<UserInfoDto>>();
        var connection = await signalConnector.GetConnection();
        connection.On<string, int>("LoggedIn", (token, id) =>
        {
            connection.Remove("LoggedIn");
            taskCompletionSource.SetResult(new ApiRequestResult<UserInfoDto>
            {
                Data = new UserInfoDto
                {
                    Id = id,
                    Token = token,
                    Name = login,
                },
                ErrorMessage = id < 0 ? "Failed to login" : "",
            });
        });
        
        await connection.InvokeAsync("Login", login, password);
        
        return await taskCompletionSource.Task;
    }

    public override async Task<ApiRequestResult<bool>> Logout()
    {
        return new ApiRequestResult<bool>()
        {
            Data = true, 
        };
    }

    public override async Task<ApiRequestResult<GameLobbyDto>> CreateGameRoom(string roomName)
    {
        var taskCompletionSource = new TaskCompletionSource<ApiRequestResult<GameLobbyDto>>();
        var connection = await signalConnector.GetConnection();
        connection.On<int, string>("RoomCreated", (id, invitationCode) =>
        {
            connection.Remove("RoomCreated");
            taskCompletionSource.SetResult(new ApiRequestResult<GameLobbyDto>
            {
                Data = new GameLobbyDto
                {
                    RoomId = "",
                    ConnectToken = invitationCode,
                },
                ErrorMessage = invitationCode.Length > 4 ? "Failed to create room" : "",
            });
        });
        
        await connection.InvokeAsync("CreateRoom", roomName, UserData.Id);
        
        return await taskCompletionSource.Task;
    }

    public override async Task<ApiRequestResult<GameLobbyDto>> JoinGameRoom(string token)
    {
        var taskCompletionSource = new TaskCompletionSource<ApiRequestResult<GameLobbyDto>>();
        var connection = await signalConnector.GetConnection();
        connection.On<int>("JoinedRoomPersonal", (roomId) =>
        {
            connection.Remove("JoinedRoomPersonal");
            taskCompletionSource.SetResult(new ApiRequestResult<GameLobbyDto>
            {
                Data = new GameLobbyDto
                {
                    RoomId = "",
                    ConnectToken = token,
                },
                ErrorMessage = roomId < 0 ? "Failed to join room" : "",
            });
        });
        
        await connection.InvokeAsync("JoinRoom", token, UserData.Id);
        
        return await taskCompletionSource.Task;
    }

    public override async Task<ApiRequestResult<GameLobbyDto>> GetRoomInfo(string roomId)
    {
        var taskCompletionSource = new TaskCompletionSource<ApiRequestResult<GameLobbyDto>>();
        var connection = await signalConnector.GetConnection();
        connection.On<string>("RoomInfo", roomJson =>
        {
            connection.Remove("RoomInfo");
            var roomDto = ParseRoomDtoFromJson(roomJson);
            taskCompletionSource.SetResult(new ApiRequestResult<GameLobbyDto>
            {
                Data = new GameLobbyDto
                {
                    RoomId = "",
                    ConnectToken = roomDto.invitationCode,
                    Players = roomDto.members
                },
                ErrorMessage = string.IsNullOrEmpty(roomId) ? "Failed to join room" : "",
            });
        });
        
        await connection.InvokeAsync("GetRoomInfo", roomId);
        
        return await taskCompletionSource.Task;
    }

    public override void LeaveGameRoom(string id)
    {
        throw new System.NotImplementedException();
    }

    private RoomDto ParseRoomDtoFromJson(string roomJson)
    {
        return JsonUtility.FromJson<RoomDto>(roomJson);
    }
}
