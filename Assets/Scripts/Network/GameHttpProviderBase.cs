using System.Threading.Tasks;
using Dto;
using Network;
using UnityEngine;

public abstract class GameHttpProviderBase: MonoBehaviour, IGameHttpApiProvider
{
    public abstract Task<ApiRequestResult<UserInfoDto>> Register(string login, string password);
    public abstract Task<ApiRequestResult<UserInfoDto>> Login(string login, string password);

    public abstract Task<ApiRequestResult<bool>> Logout();
    public abstract Task<ApiRequestResult<GameLobbyDto>> CreateGameRoom(string roomName);
    public abstract Task<ApiRequestResult<GameLobbyDto>> JoinGameRoom(string token);

    public abstract Task<ApiRequestResult<GameLobbyDto>> GetRoomInfo(string roomId);
    public abstract Task<ApiRequestResult<StatsDto>> GetMyStats();
    public abstract void LeaveGameRoom(string id);
}
