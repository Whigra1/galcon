using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Microsoft.AspNetCore.SignalR.Client;
using Unity.VisualScripting;

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
    }

    private async void SubscribeToEvents()
    {
        var connection = await signalConnector.GetConnection();
        connection.On<int, int, int>("SendShips", (from, to, amount) =>
        {
            var planet1 = _planetRegistry.FindPlanetById(from);
            var planet2 = _planetRegistry.FindPlanetById(to);
            unityContext.Post(_ => planet1.SpawnShips(planet2.transform, amount), null); 
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
        WinText.text = $"Player {winner.id} wins the game!";
    }

    public void AddPlanet(Planet planet)
    {
        _planetRegistry.AddPlanet(planet);
    }

}
