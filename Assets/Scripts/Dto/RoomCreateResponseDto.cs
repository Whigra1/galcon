using UnityEngine;

namespace Dto
{
    [System.Serializable]
    public class RoomCreateResponseDto
    {
        [SerializeField] public string roomId;
        [SerializeField] public string invitationToken;
    }
}