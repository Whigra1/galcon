using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomDto
{
    [SerializeField] public int id;
    [SerializeField] public string name = "";
    [SerializeField] public List<PlayerDto> users = new();
    [SerializeField] public string invitationCode = "";
}
