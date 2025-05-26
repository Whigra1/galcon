using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using Microsoft.AspNetCore.SignalR.Client;
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
    public SignalConnectorBase signalConnector;
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
        signalConnector.OnPlayerJoin(async playerName =>
        {
            if (players.Find(p => p.text == playerName) != null) return;
            var rInfo = await mockHttpProvider.GetRoomInfo(RoomInfo.Id);
            unityContext.Post(_ =>
            {
                ClearPlayers();
                ShowPlayers(rInfo.Data.Players ?? new List<PlayerDto>());
            }, null);
        });
        
        signalConnector.OnPlayerLeft(async (roomId, playerName) =>
        {
            var rInfo = await mockHttpProvider.GetRoomInfo(RoomInfo.Id);
            unityContext.Post(_ =>
            {
                ClearPlayers();
                ShowPlayers(rInfo.Data.Players ?? new List<PlayerDto>());
            }, null);
        });

        signalConnector.OnLoadGame(isOk =>
        {
            if (isOk) unityContext.Post(_ => SceneManager.LoadScene(1), null);
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

    public void OnDestroy()
    {
        signalConnector.RemoveGameLobbyMethods();
    }

    public void OnStart()
    {
        if (!RoomInfo.IsHost) return;
        signalConnector.RemoveGameLobbyMethods();
        signalConnector.StartGame();
        SceneManager.LoadScene(1);
    }

    public void Leave()
    {
        signalConnector.RemoveGameLobbyMethods();

        mockHttpProvider.LeaveGameRoom(RoomInfo.Id);

        RoomInfo.Clear();

        ClearPlayers();

        gameObject.SetActive(false);
        onlineMenu.SetActive(true);
    }

}
