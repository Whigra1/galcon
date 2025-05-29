using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventArgs;
using Network.Signal;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool _isGameRunning = true; // Flag to stop the coroutine when the game ends
    private readonly PlanetRegistry _planetRegistry = new();
    public TMP_Text WinText;
    public GameObject WinMenu;
    public PlanetSpawnerBase planetSpawner;
    public GameConnectorBase signalConnector;
    private SynchronizationContext unityContext;
    private void Awake()
    {
        unityContext = SynchronizationContext.Current;
    }
    async Task Start()
    {
        _planetRegistry.AddPlanets(await planetSpawner.SpawnPlanets());
        SetUserId();
        SubscribeToEvents();
        // StartCoroutine(CheckGameEndConditionCoroutine());
    }

    private void SetUserId() // kostyl
    {
        var player = RoomInfo.Players?.Values.FirstOrDefault(p => p.Name == UserData.Name);
        if (player is not null)
        {
            UserData.Id = player.Id;
        }
    }

    private async void SubscribeToEvents()
    {
        signalConnector.OnSendShips(async (obj) =>
        {
            var data = ((JObject)obj).ToObject<FleetSentArgs>();
            var planet1 = _planetRegistry.FindPlanetById(data.DeparturePlanet.Id.ToString());
            var planet2 = _planetRegistry.FindPlanetById(data.DestinationPlanet.Id.ToString());
            if (planet1 == null || planet2 == null) return;
            unityContext.Post(_ => planet1.SpawnShips(planet2.transform, data.DeparturePlanet.Ships), null);
        });
        signalConnector.OnPlanetProducedShips(obj =>
        {
            var planetInfo = ((JObject)obj).ToObject<PlanetProduceShipsEvent>();
            var planet = _planetRegistry.FindPlanetById(planetInfo.PlanetId);
            if (planet) planet.shipsAmount = planetInfo.Ships;
        });
        signalConnector.OnShipsArrived(obj =>
        {
            Debug.Log("Ships arrived");
        });
        signalConnector.OnGameEnded(obj =>
        {
            var winnerGuid = ((JObject)obj)["winnerId"];
            var winner = RoomInfo.Players[winnerGuid.ToString()];
            unityContext.Post(_ => EndGame(new Player(winner.Name, winner.Id)), null);
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
        signalConnector.CloseConnection();
    }
}
