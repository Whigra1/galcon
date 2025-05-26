using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dto;
using Network;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class CalgonHttpProvider: GameHttpProviderBase
{
    public static readonly string ApiUrl = "http://localhost:5000";
    public override async Task<ApiRequestResult<UserInfoDto>> Register(string login, string password)
    {
        // Create the payload object
        var payload = new
        {
            email = login,
            password
        };

        var jsonPayload = JsonConvert.SerializeObject(payload);
        using var request = CreateRequest("/register", "POST", jsonPayload);
        await request.SendWebRequest();

        // Check the result of the request
        if (request.result == UnityWebRequest.Result.Success)
        {
            // Parse the response JSON into the UserInfoDto object
            return new ApiRequestResult<UserInfoDto>
            {
                Data = new UserInfoDto
                {
                    Id = -1,
                    Token = "",
                    Name = "",
                },
                ErrorMessage = "",
            };
        }
        return new ApiRequestResult<UserInfoDto>
        {
            Data = new UserInfoDto
            {
                Id = -1,
                Token = "",
                Name = "",
            },
            ErrorMessage = "Failed to register",
        };
    }

    public override async Task<ApiRequestResult<UserInfoDto>> Login(string login, string password)
    {
        var payload = new
        {
            email = login,
            password
        };

        var jsonPayload = JsonConvert.SerializeObject(payload);
        using var request = CreateRequest("/login", "POST", jsonPayload);
        await request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            // Parse the response JSON into the UserInfoDto object

            var responseJson = JsonConvert.DeserializeObject<LoginResponceDto>(request.downloadHandler.text);
            
            return new ApiRequestResult<UserInfoDto>
            {
                Data = new UserInfoDto
                {
                    Id = 1,
                    Token = responseJson.accessToken,
                    Name = login,
                },
                ErrorMessage = "",
            };
        }
        return new ApiRequestResult<UserInfoDto>
        {
            Data = new UserInfoDto
            {
                Id = -1,
                Token = "",
                Name = "",
            },
            ErrorMessage = "Failed to login",
        };
    }

    public override Task<ApiRequestResult<bool>> Logout()
    {
        throw new System.NotImplementedException();
    }

    public override async Task<ApiRequestResult<GameLobbyDto>> CreateGameRoom(string roomName)
    {
        var payload = new
        {
            name = roomName
        };
        var jsonPayload = JsonConvert.SerializeObject(payload);
        using var request = CreateRequest("/rooms", "POST", jsonPayload);
        await request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            var responseJson = JsonConvert.DeserializeObject<RoomCreateResponseDto>(request.downloadHandler.text);
            return new ApiRequestResult<GameLobbyDto>
            {
                Data = new GameLobbyDto
                {
                    RoomId = responseJson.roomId,
                    ConnectToken = responseJson.invitationToken,
                },
                ErrorMessage = ""
            };
        }
        return new ApiRequestResult<GameLobbyDto>
        {
            Data = new GameLobbyDto
            {
            },
            ErrorMessage = "Failed to create room",
        };
        
    }

    public override async Task<ApiRequestResult<GameLobbyDto>> JoinGameRoom(string token)
    {
        using var request = CreateRequest($"/rooms/join/{token}", "GET", "");
        await request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            // Parse the response JSON into the UserInfoDto object

            var responseJson = JsonConvert.DeserializeObject<JoinGameResponseDto>(request.downloadHandler.text);
            
            return new ApiRequestResult<GameLobbyDto>
            {
                Data = new GameLobbyDto
                {
                    RoomId = responseJson.roomId,
                    ConnectToken = token
                },
                ErrorMessage = "",
            };
        }
        return new ApiRequestResult<GameLobbyDto>
        {
            Data = new GameLobbyDto
            {
                RoomId = ""
            },
            ErrorMessage = "Failed to join room",
        };
    }

    public override async Task<ApiRequestResult<GameLobbyDto>> GetRoomInfo(string roomId)
    {
        using var request = CreateRequest($"/rooms/{roomId}", "GET", "");
        await request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            var responseJson = JsonConvert.DeserializeObject<RoomDto>(request.downloadHandler.text);
            return new ApiRequestResult<GameLobbyDto>
            {
                Data = new GameLobbyDto
                {
                    RoomId = responseJson.id,
                    ConnectToken = responseJson.invitationCode,
                    Players = responseJson.members
                },
                ErrorMessage = "",
            };
        }
        return new ApiRequestResult<GameLobbyDto>
        {
            Data = new GameLobbyDto
            {
                RoomId = "",
                ConnectToken = "",
                Players = new List<PlayerDto>()
            },
            ErrorMessage = "Failed to get room info",
        };
    }

    public override async void LeaveGameRoom(string roomId)
    {
        using var request = CreateRequest($"/leave/{roomId}", "POST", "");
        await request.SendWebRequest();
    }

    private UnityWebRequest CreateRequest(string endpoint, string method, string jsonPayload)
    {
        var request = new UnityWebRequest(ApiUrl + endpoint, method);

        if (!string.IsNullOrEmpty(UserData.Token))
            request.SetRequestHeader("Authorization", $"Bearer {UserData.Token}");
        if (!string.IsNullOrEmpty(jsonPayload))
        {
            var bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        }
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        return request;
    }
}
