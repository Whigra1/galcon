using System;
using Network.Signal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public GameConnectorBase signalConnector;
    public GameHttpProviderBase gameHttpProvider;
    public GameObject menuPanel;
    private bool _isMenuOpen;

    public void Start ()
    {
        ResumeGame();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            ToggleMenu();
        }
    }

    public void ResumeGame()
    {
        _isMenuOpen = false;
        menuPanel.SetActive(false);
        Time.timeScale = 1f;
    }
    
    public async void BackToMainMenu()
    {
        await signalConnector.CloseConnection();
        gameHttpProvider.LeaveGameRoom(RoomInfo.Id);
        RoomInfo.Clear();
        SceneManager.LoadScene(0);
    }
    
    void ToggleMenu()
    {
        _isMenuOpen = !_isMenuOpen;
        menuPanel.SetActive(_isMenuOpen);
        Time.timeScale = _isMenuOpen
            ? 0f // Pause the game
            : 1f; // Resume the game
    }
}
