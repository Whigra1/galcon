using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class OnlineMenuScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public MockHttpProvider mockHttpProvider;
    public TMP_InputField gameNameInput;
    public TMP_InputField tokenInput;
    public GameObject gameLobby;
    public TMP_Text errorText;
    
    public void OnCreateNewGame()
    {
        mockHttpProvider.CreateGameRoom(gameNameInput.text);
    }

    public async void OnConnect()
    {
        if (string.IsNullOrEmpty(tokenInput.text))
        {
            errorText.text = "Token is empty";
            return;
        }
        
        var response = await mockHttpProvider.JoinGameRoom(tokenInput.text);
        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            errorText.text = response.ErrorMessage;
        }
        
        errorText.text = "";
        gameNameInput.text = "";
        tokenInput.text = "";
        
        PlayerPrefs.SetString("connection_token", response.Data.ConnectToken);
        PlayerPrefs.SetString("room_id", response.Data.RoomId);
        gameObject.SetActive(false);
        gameLobby.SetActive(true);
        
    }
}
