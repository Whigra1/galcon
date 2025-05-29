using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player
{
    public string Name { get; set; } = "";
    public string id { get; private set; } = "";
    public Color color { get; } = Random.ColorHSV();
    public Player(string name, string userID, Color? color = null)
    {
        id = userID;
        Name = name;
        if (color != null)
        {
            this.color = (Color) color;
        }
    }

    public static Player NoOne()
    {
        return new Player("No one", Guid.NewGuid().ToString(), Color.gray);
    }
}
