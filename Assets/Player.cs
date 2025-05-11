using UnityEngine;

public class Player
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private string _name;
    public int id { get; private set; } = 0;
    public Color color { get; } = Random.ColorHSV();
    public Player(string name, int userID)
    {
        id = userID;
        _name = name;
    } 
}
