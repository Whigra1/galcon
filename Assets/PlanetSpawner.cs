using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlanetSpawner : MonoBehaviour
{
    public float maxRadius = 35f;          // Maximum radius of the circle
    public float radiusMean = 2f;         // Mean for the radius
    public float radiusStdDev = 1f;       // Standard deviation for the radius
    public float angleMean = 0f;          // Mean for the angle (in degrees)
    public float angleStdDev = 60f;       // Standard deviation for the angle (in degrees)

    public int planetAmount = 50;
    
    public GameObject planetPrefab;
    
    public List<GameObject> spawners = new ();
    public int distanceBetweenPlanets = 5;
    
    [FormerlySerializedAs("PlayersAmount")] public int playersAmount = 2;
    
    void Start()
    {
        var planetPositions = new List<Vector3>();
        for (var i = 0; i < Math.Min(playersAmount, spawners.Count); i++)
        {
            var player = new Player($"Player {i + 1}", i + 1);
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
                
                planetPositions.Add(point);
                var planetGameObject = Instantiate(planetPrefab, point, Quaternion.identity);
                var planet = planetGameObject.GetComponent<Planet>();
                if (planet is not null)
                {
                    planet.transform.position = point;
                    planet.ownerPlayer = player;
                    var spriteRenderer = planetGameObject.GetComponent<SpriteRenderer>();
                    spriteRenderer.color = planet.ownerPlayer.color;
                }
            }
        }
    }


    private bool IsPointValid (Vector3 point, List<Vector3> planetPositions)
    {
        foreach (var existingPosition in planetPositions)
        {
            // Minimum distance = sum of radii (to prevent overlap)
            float minDistance = 2 * distanceBetweenPlanets;
            if (Vector2.Distance(existingPosition, point) < minDistance)
            {
                return false;
            }
        }

        return true;
    }
    
    private Vector3 GenerateGaussianRandomPosition (Vector3 center)
    {
        var radius = Mathf.Abs(GenerateGaussianRandom(radiusMean, radiusStdDev));
        radius = Mathf.Clamp(radius, 0, maxRadius); // Clamp radius to avoid exceeding max radius

        // Generate random angle (Gaussian distribution)
        var angle = GenerateGaussianRandom(angleMean, angleStdDev);
        angle = angle % 360; // Keep angle within 0-360 degrees

        // Convert polar coordinates (radius, angle) to Cartesian
        var angleInRadians = angle * Mathf.Deg2Rad;
        var x = center.x + radius * Mathf.Cos(angleInRadians);
        var y = center.y + radius * Mathf.Sin(angleInRadians);
        return new Vector3(x, y, 0);
    }
    

    // Function to generate Gaussian random values (mean and standard deviation)
    float GenerateGaussianRandom(float mean, float standardDeviation)
    {
        var u1 = Random.value;
        var u2 = Random.value;
        var z0 = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Cos(2.0f * Mathf.PI * u2);
        return z0 * standardDeviation + mean;
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
