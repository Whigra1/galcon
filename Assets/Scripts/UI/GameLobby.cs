using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameLobby : MonoBehaviour
{
    public MockHttpProvider mockHttpProvider;
    public TMP_Text playerNamePrefab;

    public GameObject playersParentComponent;
    public List<GameObject> players = new();
    public GameObject onlineMenu;
    
    public async void OnEnable()
    {
        var roomInfo = await mockHttpProvider.GetRoomInfo(PlayerPrefs.GetInt("room_id"));
        for (var i = 0; i < roomInfo.Data.Players.Count; i++)
        {
            var dataPlayer = roomInfo.Data.Players[i];
            var player = Instantiate(playerNamePrefab, transform);
            player.text = dataPlayer.Name.Length > 18 ? dataPlayer.Name[..15] + "..." : dataPlayer.Name;
            player.transform.position = new Vector3(
                player.transform.position.x + 10,
                player.transform.position.y - 35 * i, // height of text
                0
            );
            player.gameObject.SetActive(true);
            players.Add(player.gameObject);
        }
    }

    public void Leave()
    {
        gameObject.SetActive(false);
        foreach (var player in players) { Destroy(player); }
        players.Clear();
        onlineMenu.SetActive(true);
    }

}
