using System;
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

    public Player ownerPlayer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        size = Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(size, size, 1);
    }

    // Update is called once per frame
    void Update()
    {
        var delta = Time.deltaTime;
        GrowUpBuffer += delta * shipGrowsFactor * size;
        if (GrowUpBuffer > 1)
        {
            shipsAmount += 1;
            GrowUpBuffer -= 1;
        }
        ShipAmountText.text = $"{shipsAmount}";
    }

    public void SpawnShips(Transform target)
    {
        var halfShipsAmount = (int) Math.Floor(shipsAmount / 2d);
        var targetVelocity = target.transform.position - transform.position;
     
        for (var i = 0; i < halfShipsAmount; i++)
        {
            var randomVector = Random.insideUnitCircle * 2;
            var position = transform.position + targetVelocity.normalized * (size + 1) + new Vector3(randomVector.x, randomVector.y, 0);
            var newTriangle = Instantiate(shipPrefab, position, Quaternion.identity);
            // var t = newTriangle.GetComponent<Seek>();
            var t = newTriangle.GetComponent<AllInOneSteering>();
            var triangle = newTriangle.GetComponent<Triangle>();
            var spriteRenderer = newTriangle.GetComponent<SpriteRenderer>();
            triangle.owner = ownerPlayer;
            t.seek = target.transform;
            spriteRenderer.color = ownerPlayer.color;
        }
        shipsAmount -= halfShipsAmount;
    }

    public bool AcceptShip(Triangle ship)
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
   
}
