using System.Threading.Tasks;
using Network;
using UnityEngine;

public interface IGameHttpApiProvider
{
    public Task<ApiRequestResult<UserInfoDto>> Register (string login, string password);
    public Task<ApiRequestResult<UserInfoDto>> Login (string login, string password);
    public Task<ApiRequestResult<bool>> Logout ();

    public Task<ApiRequestResult<GameLobbyDto>> CreateGameRoom (string name);
    public Task<ApiRequestResult<GameLobbyDto>>  JoinGameRoom (string token);
    public Task<ApiRequestResult<GameLobbyDto>>  GetRoomInfo(int roomId);

    public void LeaveGameRoom(int id);
}
