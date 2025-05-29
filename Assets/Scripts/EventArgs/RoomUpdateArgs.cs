using System.Collections.Generic;

namespace EventArgs
{
    public class RoomUpdateArgs
    {
        public string Name { get; set; }
        public string InvitationCode { get; set; }
        public RoomStatus Status { get; set; }
        public string RoomCreatorId { get; set; }
        public List<RoomMember> Members { get; set; } = new();
    }
    
    public class RoomMember
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
    
    public enum RoomStatus
    {
        Open,
        InGame
    }
}