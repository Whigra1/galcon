using UnityEngine;

[System.Serializable]
public class PlayerDto
{
    [SerializeField] public int id;
    [SerializeField] public string name = "";

    public int Id => id;
    public string Name => name;
}
