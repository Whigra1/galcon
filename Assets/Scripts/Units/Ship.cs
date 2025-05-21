using System;
using Unity.VisualScripting;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public Player owner;
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
