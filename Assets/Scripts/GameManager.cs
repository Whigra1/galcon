using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool _isGameRunning = true; // Flag to stop the coroutine when the game ends
    private readonly PlanetRegistry _planetRegistry = new();
    public TMP_Text WinText;
    public GameObject WinMenu;
    public PlanetSpawnerBase planetSpawner;
    public SignalConnectorBase signalConnector;
    private SynchronizationContext unityContext;
    private void Awake()
    {
        unityContext = SynchronizationContext.Current;
    }
    async Task Start()
    {
        _planetRegistry.AddPlanets(await planetSpawner.SpawnPlanets());
        SubscribeToEvents();
        StartCoroutine(CheckGameEndConditionCoroutine());
        signalConnector.Ready(UserData.Id, RoomInfo.Id);
    }

    private async void SubscribeToEvents()
    {
        signalConnector.OnSendShips(async (from, to, amount) =>
        {
            var planet1 = _planetRegistry.FindPlanetById(from);
            var planet2 = _planetRegistry.FindPlanetById(to);
            unityContext.Post(_ => planet1.SpawnShips(planet2.transform, amount), null);
        });
        
        signalConnector.OnGameStarted(isOk =>
        {
            if (isOk)
            {
                GameState.IsGameRunning = true;
            }
        });
    }
    
    
    private IEnumerator CheckGameEndConditionCoroutine()
    {
        while (_isGameRunning)
        {
            CheckGameEndCondition();
            yield return new WaitForSeconds(1f); // Wait for 1 second before checking again
        }
    }

    private void CheckGameEndCondition()
    {
        var owners = _planetRegistry.GetPlanetOwners();
        if (owners.Count == 1)
        {
            EndGame(_planetRegistry[0].ownerPlayer);
        }
    }

    private void EndGame(Player winner)
    {
        _isGameRunning = false; // Stop the coroutine
        WinMenu.SetActive(true);
        WinText.text = $"Player {winner.Name} wins the game!";
    }

    public void AddPlanet(Planet planet)
    {
        _planetRegistry.AddPlanet(planet);
    }

    private void OnDestroy()
    {
        signalConnector.RemoveGameManagerMethods();
    }
}
