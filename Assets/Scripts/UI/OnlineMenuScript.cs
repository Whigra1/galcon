using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class OnlineMenuScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameHttpProviderBase mockHttpProvider;
    public TMP_InputField gameNameInput;
    public TMP_InputField tokenInput;
    public GameObject gameLobby;
    public TMP_Text errorText;
    
    public async void OnCreateNewGame()
    {
        var response = await mockHttpProvider.CreateGameRoom(gameNameInput.text);
        if (string.IsNullOrEmpty(response.ErrorMessage))
        {
            errorText.text = "";
            gameNameInput.text = "";
            
            RoomInfo.Id = response.Data.RoomId;
            RoomInfo.Name = gameNameInput.text;
            RoomInfo.InvitatationToken = response.Data.ConnectToken;
            RoomInfo.IsHost = true;

            gameObject.SetActive(false);
            gameLobby.SetActive(true);
            return;
        }
        
        errorText.text = response.ErrorMessage;
    }

    public async void OnConnect()
    {
        if (string.IsNullOrEmpty(tokenInput.text))
        {
            errorText.text = "Token is empty";
            return;
        }
        
        var response = await mockHttpProvider.JoinGameRoom(tokenInput.text);
        
        if (!string.IsNullOrEmpty(response.ErrorMessage) || string.IsNullOrEmpty(response.Data.RoomId))
        {
            errorText.text = response.ErrorMessage;
            return;
        }
        
        errorText.text = "";
        gameNameInput.text = "";
        tokenInput.text = "";
        
        RoomInfo.Id = response.Data.RoomId;
        RoomInfo.Name = gameNameInput.text;
        RoomInfo.InvitatationToken = response.Data.ConnectToken;
        RoomInfo.IsHost = false;
        gameObject.SetActive(false);
        gameLobby.SetActive(true);
    }
}
