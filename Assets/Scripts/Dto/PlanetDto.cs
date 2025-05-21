using UnityEngine;

[System.Serializable]

public class PlanetDto
{
    [SerializeField] public int id;
    [SerializeField] public double x;
    [SerializeField] public double y;
    [SerializeField] public int size;
    [SerializeField] public int ownerId;
    [SerializeField] public string color;
}
