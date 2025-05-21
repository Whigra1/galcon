using System.Collections.Generic;
using System.Threading.Tasks;
using Network;
using UnityEngine;
using Microsoft.AspNetCore.SignalR.Client;
public class MockHttpProvider : MonoBehaviour, IGameHttpApiProvider
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public SignalConnectorBase signalConnector;
    
    private static ApiRequestResult<GameLobbyDto> mockLobby = new ApiRequestResult<GameLobbyDto>()
    {
        Data = new GameLobbyDto
        {
            ConnectToken = "token",
            Players = new List<PlayerDto>()
            {
                new()
                {
                    Id = 1,
                    Name = "Player1", 
                },
                new()
                {
                    Id = 2,
                    Name = "Player2", 
                },
            }
        }, 
    };
    
    public async Task<ApiRequestResult<UserInfoDto>> Register(string login, string password)
    {
        var taskCompletionSource = new TaskCompletionSource<ApiRequestResult<UserInfoDto>>();
        var connection = await signalConnector.GetConnection();
        connection.On<string, int>("Registered", (token, id) =>
        {
            taskCompletionSource.SetResult(new ApiRequestResult<UserInfoDto>
            {
                Data = new UserInfoDto
                {
                    Id = id,
                    Token = token,
                    Name = login,
                }
            });
        });
        
        await connection.InvokeAsync("Register", login, password);
        
        return await taskCompletionSource.Task;
    }

    public async Task<ApiRequestResult<UserInfoDto>> Login(string login, string password)
    {
        var taskCompletionSource = new TaskCompletionSource<ApiRequestResult<UserInfoDto>>();
        var connection = await signalConnector.GetConnection();
        connection.On<string, int>("LoggedIn", (token, id) =>
        {
            taskCompletionSource.SetResult(new ApiRequestResult<UserInfoDto>
            {
                Data = new UserInfoDto
                {
                    Id = id,
                    Token = token,
                    Name = login,
                }
            });
        });
        
        await connection.InvokeAsync("Login", login, password);
        
        return await taskCompletionSource.Task;
    }

    public async Task<ApiRequestResult<bool>> Logout()
    {
        return new ApiRequestResult<bool>()
        {
            Data = true, 
        };
    }

    public async Task<ApiRequestResult<GameLobbyDto>> CreateGameRoom(string name)
    {
        return mockLobby;
    }

    public async Task<ApiRequestResult<GameLobbyDto>> JoinGameRoom(string token)
    {
        return mockLobby;
    }

    public async Task<ApiRequestResult<GameLobbyDto>> GetRoomInfo(int roomId)
    {
        return mockLobby;
    }

    public void LeaveGameRoom(int id)
    {
        throw new System.NotImplementedException();
    }
}
