using UnityEngine;

public class Player
{
    public string Name { get; set; } = "";
    public int id { get; private set; } = 0;
    public Color color { get; } = Random.ColorHSV();
    public Player(string name, int userID, Color? color = null)
    {
        id = userID;
        Name = name;
        if (color != null)
        {
            this.color = (Color) color;
        }
    } 
}
