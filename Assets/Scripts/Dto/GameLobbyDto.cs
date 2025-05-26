using System.Collections.Generic;
using UnityEngine;

public class GameLobbyDto
{
    public string ConnectToken { get; set; }
    public string RoomId { get; set; }
    public List<PlayerDto> Players { get; set; }
}
