using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Planet : MonoBehaviour
{
    [FormerlySerializedAs("ShipsAmount")] public int shipsAmount = 0;

    [FormerlySerializedAs("ShipGrowsFactor")] public float shipGrowsFactor = 1;

    public GameObject shipPrefab;

    public TMP_Text ShipAmountText;
    public double GrowUpBuffer { get; set; } = 0;

    public int size = 5;

    public int maxSize = 15;
    public int minSize = 5;
    public int batchSize = 30;
    public float delayBetweenBatches = 0.2f;
    public Player ownerPlayer;
    public string id;
    
    void Update()
    {
        // if (!GameState.IsGameRunning) return;

        var delta = Time.deltaTime;
        GrowUpBuffer += delta * shipGrowsFactor * size;
        if (GrowUpBuffer > 1)
        {
            shipsAmount += 1;
            GrowUpBuffer -= 1;
        }
        ShipAmountText.text = $"{shipsAmount}";
    }
    
    public void SetSize (int newSize)
    {
        size = newSize;
        transform.localScale = new Vector3(size, size, 1);
    }

    public int SpawnShips(Transform target, int amount = -1)
    {
        var halfShipsAmount = amount > 0 ? amount : (int) Math.Floor(shipsAmount / 2d);
        var targetVelocity = target.transform.position - transform.position;
        StartCoroutine(SpawnShipsInBatches(target, targetVelocity, halfShipsAmount));
        shipsAmount -= halfShipsAmount;
        return halfShipsAmount;
    }
    
    private IEnumerator SpawnShipsInBatches(Transform target, Vector3 targetVelocity, int spawnAmount)
    {
        var leftToSpawn = spawnAmount;
        
        while (leftToSpawn > 0)
        {
            var maxToSpawnInBatch = Math.Min(leftToSpawn, batchSize);
            for (int i = 0; i < maxToSpawnInBatch; i++)
            {
                SpawnShip(target, targetVelocity);
            }
            leftToSpawn -= maxToSpawnInBatch;
            yield return new WaitForSeconds(delayBetweenBatches);
        }
    }


    private void SpawnShip(Transform target, Vector3 targetVelocity)
    {
        var randomVector = Random.insideUnitCircle;
        var position = transform.position + targetVelocity.normalized * (size + 1) + new Vector3(randomVector.x, randomVector.y, 0);
        var newTriangle = Instantiate(shipPrefab, position, Quaternion.identity);
        var t = newTriangle.GetComponent<AllInOneSteering>();
        var triangle = newTriangle.GetComponent<Ship>();
        var spriteRenderer = newTriangle.GetComponent<SpriteRenderer>();
        triangle.owner = ownerPlayer;
        t.seek = target.transform;
        spriteRenderer.color = ownerPlayer.color;
    }

    public bool AcceptShip(Ship ship)
    {
        if (ship is null) return false;
        var seekTarget = ship.GetComponent<AllInOneSteering>().seek;
        if (seekTarget.position != transform.position) return false;
        
        if (ship.owner.id == ownerPlayer.id)
            shipsAmount += 1;
        else
            shipsAmount -= 1;
        
        if (shipsAmount <= 0)
        {
            ownerPlayer = ship.owner; 
            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = ownerPlayer.color;
        }
        return true;
    }

    private static int _id = 0;
    private static List<string> colors = new List<string> { "red", "green", "blue", "yellow", "orange" };
    
}
