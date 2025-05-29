using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventArgs;
using TMPro;
using UnityEngine;
using Microsoft.AspNetCore.SignalR.Client;
using Network.Signal;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLobby : MonoBehaviour
{
    public GameHttpProviderBase mockHttpProvider;
    public TMP_Text playerNamePrefab;
    public TMP_Text connectionToken;
    public GameObject playersParentComponent;
    public List<TMP_Text> players = new();
    public Button startButton;
    public GameObject onlineMenu;
    public GameConnectorBase gameConnector;
    public LobbyConnectorBase LobbyConnector;
    private SynchronizationContext unityContext;
    
    public void Awake()
    {
        unityContext = SynchronizationContext.Current;
    }

    public async void OnEnable()
    {
        startButton.enabled = RoomInfo.IsHost;
        
        var roomInfo = await mockHttpProvider.GetRoomInfo(RoomInfo.Id);
        connectionToken.text = $"Connection token: {roomInfo.Data.ConnectToken}";
        ShowPlayers(roomInfo.Data.Players ?? new List<PlayerDto>());
        await Task.WhenAll(
            LobbyConnector.StartConnection(),
            gameConnector.StartConnection()
        );
        await LobbyConnector.OnRoomUpdate(roomObj =>
        {
            var updateArgs = ((JObject) roomObj).ToObject<RoomUpdateArgs>();
            unityContext.Post(_ =>
            {
                ClearPlayers();
                ShowPlayers(updateArgs.Members.Select(m => new PlayerDto
                {
                    userId = m.UserId,
                    userName = m.UserName
                }).ToList());
            }, null);
        });

        gameConnector.OnGameStarted(jObj =>
        {
            var res = ((JObject) jObj).ToObject<GameStartedArgs>();
            RoomInfo.Planets = res.Planets;
            RoomInfo.Players = res.Players;
            unityContext.Post(_ => SceneManager.LoadScene(1), null);
        });
    }

    public void ShowPlayers (List<PlayerDto> playersDtos)
    {
        if (gameObject == null || gameObject.IsDestroyed()) return;
        for (var i = 0; i < playersDtos.Count; i++)
        {
            var dataPlayer = playersDtos[i];
            var player = Instantiate(playerNamePrefab, transform);
            if (!player) return;
            player.text = dataPlayer.Name.Length > 18 ? dataPlayer.Name[..15] + "..." : dataPlayer.Name;
            player.transform.position = new Vector3(
                player.transform.position.x + 10,
                player.transform.position.y - 35 * i, // height of text
                0
            );
            player.gameObject.SetActive(true);
            players.Add(player);
        }
    }
    
    public void ClearPlayers()
    {
        if (gameObject == null || gameObject.IsDestroyed()) return;
        foreach (var player in players ?? new List<TMP_Text>())
        {
            if (player) Destroy(player.gameObject);
        }
        players.Clear();
    }

    public async void OnDestroy()
    {
        await LobbyConnector.CloseConnection();
    }

    public void OnStart()
    {
        if (!RoomInfo.IsHost) return;
        LobbyConnector.CloseConnection();
        gameConnector.StartGame();
        // SceneManager.LoadScene(1);
    }

    public async void Leave()
    {
        await gameConnector.CloseConnection();
        await LobbyConnector.CloseConnection();
        mockHttpProvider.LeaveGameRoom(RoomInfo.Id);
        RoomInfo.Clear();
        ClearPlayers();
        gameObject.SetActive(false);
        onlineMenu.SetActive(true);
    }

}
