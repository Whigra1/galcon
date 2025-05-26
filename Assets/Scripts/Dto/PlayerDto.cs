using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class PlayerDto
{
    [SerializeField] public string userId;
    [SerializeField] public string userName = "";

    public string UserId => userId;
    public string Name => userName;
}
