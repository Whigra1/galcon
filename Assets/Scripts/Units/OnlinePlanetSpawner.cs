using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;

[System.Serializable]
public class PlanetWrapper
{
    [SerializeField] public List<PlanetDto> planets;
}

public class OnlinePlanetSpawner : PlanetSpawnerBase
{
    private SynchronizationContext unityContext;
    public GameObject planetPrefab;
    public SignalConnectorBase signalConnector; 
    private readonly Dictionary<int, Player> _players = new();
    
    private void Awake()
    {
        unityContext = SynchronizationContext.Current;
    }

    
    public override async Task<List<Planet>> SpawnPlanets()
    {
        var taskCompletionSource = new TaskCompletionSource<List<Planet>>();
        var connection = await signalConnector.GetConnection();
        connection.On<string>("Planets", planetJson =>
        {
            var planets = new List<Planet>();
            var planetWrapper = ParsePlanetsFromJson(planetJson);
            unityContext ??= SynchronizationContext.Current;
            unityContext.Post(_ =>
            {
                foreach (var planetDto in planetWrapper.planets)
                {
                    planets.Add(CreatePlanetFromDto(planetDto));
                }
                taskCompletionSource.SetResult(planets);
            }, null);
            connection.Remove("Planets");
        });
        await connection.InvokeAsync("GetPlanets");
        return await taskCompletionSource.Task;

    }
    
    private PlanetWrapper ParsePlanetsFromJson(string planetJson)
    {
        return JsonUtility.FromJson<PlanetWrapper>(planetJson);
    }

    private Planet CreatePlanetFromDto(PlanetDto planetDto)
    {
        var planetGameObj = Instantiate(planetPrefab);
        var planet = planetGameObj.GetComponent<Planet>();
        var spriteRenderer = planetGameObj.GetComponent<SpriteRenderer>();
        // planet.id = planetDto.id;
        planet.transform.position = new Vector3((float) planetDto.x, (float) planetDto.y, 0);
        if (!_players.ContainsKey(planetDto.ownerId))
        {
            _players[planetDto.ownerId] = new Player($"Player {planetDto.ownerId}", planetDto.ownerId.ToString(), GetColorFromDto(planetDto));
        }
        planet.SetSize(planetDto.size);
        planet.ownerPlayer = _players[planetDto.ownerId];
        spriteRenderer.color = planet.ownerPlayer.color;
        return planet;
    }

    private Color GetColorFromDto(PlanetDto planetDto)
    {
        switch (planetDto.color)
        {
            case "red": return Color.red;
            case "green": return Color.green;
            case "blue": return Color.blue;
            case "yellow": return Color.yellow;
        }
        var parsed = Enum.TryParse<Color>(planetDto.color, out var color);
        return parsed ? color : Color.white;
    }
}
