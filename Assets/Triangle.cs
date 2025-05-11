using System;
using Unity.VisualScripting;
using UnityEngine;

public class Triangle : MonoBehaviour
{
    public Player owner;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter2D (Collision2D other)
    {
        if (other.gameObject.tag == "Planet")
        {
            var planet = other.gameObject.GetComponent<Planet>(); 
            if (planet.AcceptShip(this))
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Planet")
        {
            var planet = other.gameObject.GetComponent<Planet>(); 
            if (planet.AcceptShip(this))
            {
                Destroy(gameObject);
            }
        }
    }
}
