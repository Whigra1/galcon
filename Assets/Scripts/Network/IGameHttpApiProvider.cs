using System.Threading.Tasks;
using Dto;
using Network;
using UnityEngine;

public interface IGameHttpApiProvider
{
    public Task<ApiRequestResult<UserInfoDto>> Register (string login, string password);
    public Task<ApiRequestResult<UserInfoDto>> Login (string login, string password);
    public Task<ApiRequestResult<bool>> Logout ();

    public Task<ApiRequestResult<GameLobbyDto>> CreateGameRoom (string roomName);
    public Task<ApiRequestResult<GameLobbyDto>> JoinGameRoom (string token);
    public Task<ApiRequestResult<GameLobbyDto>> GetRoomInfo(string roomId);
    public Task<ApiRequestResult<StatsDto>> GetMyStats();
    public void LeaveGameRoom(string id);
}
