using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class RoomDto
{
    [SerializeField] public string id;
    [SerializeField] public string name = "";
    [SerializeField] public List<PlayerDto> members = new();
    [SerializeField] public string invitationCode = "";
}
