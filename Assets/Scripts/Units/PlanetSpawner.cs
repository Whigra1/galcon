using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlanetSpawner : PlanetSpawnerBase
{
    public float maxRadius = 35f;          // Maximum radius of the circle
    public int planetAmount = 6;
    
    public GameObject planetPrefab;
    public GameManager gameManager;

    public List<GameObject> spawners = new ();
    public int distanceBetweenPlanets = 5;

    [FormerlySerializedAs("PlayersAmount")] public int playersAmount = 2;
    public override Task<List<Planet>> SpawnPlanets()
    {
        var planetPositions = new List<(Vector3, int)>();
        var planets = new List<Planet>(planetAmount * playersAmount);
        for (var i = 0; i < Math.Min(playersAmount, spawners.Count); i++)
        {
            var player = new Player($"Player {i + 1}", Guid.NewGuid().ToString());
            for (var j = 0; j < planetAmount; j++)
            {
                var validPosition = true;
                var maxAttempts = 1000;
                var attempts = 0;
                var point = Vector3.zero;
                do
                {
                    attempts++;
                    point = GenerateRandomPosition(spawners[i].transform.position);
                    validPosition = IsPointValid(point, planetPositions);
                } while (!validPosition && attempts < maxAttempts);

                if (!validPosition) continue;
                
                var planetGameObject = Instantiate(planetPrefab, point, Quaternion.identity);
                var planet = planetGameObject.GetComponent<Planet>();
                planetPositions.Add((point, planet.size));
                if (planet is not null)
                {
                    planet.transform.position = point;
                    planet.ownerPlayer = player;
                    var spriteRenderer = planetGameObject.GetComponent<SpriteRenderer>();
                    spriteRenderer.color = planet.ownerPlayer.color;
                    planets.Add(planet);
                }
            }
        }
        return Task.FromResult(planets); 
    }
    private bool IsPointValid (Vector3 point, List<(Vector3, int)> planetPositions)
    {
        foreach (var existingPosition in planetPositions)
        {
            // Minimum distance = sum of radii (to prevent overlap)
            float minDistance = distanceBetweenPlanets * existingPosition.Item2;
            if (Vector2.Distance(existingPosition.Item1, point) < minDistance)
            {
                return false;
            }
        }

        return true;
    }
    
    private Vector3 GenerateRandomPosition(Vector3 spawnCenter)
    {
        // Generate a random position within the spawn radius
        var angle = Random.Range(0f, 360f) * Mathf.Deg2Rad; // Random angle in radians
        var radius = Random.Range(0f, maxRadius);         // Random radius within the circle
        var x = spawnCenter.x + radius * Mathf.Cos(angle);
        var y = spawnCenter.y + radius * Mathf.Sin(angle);
        return new Vector3(x, y, 0);
    }
}
