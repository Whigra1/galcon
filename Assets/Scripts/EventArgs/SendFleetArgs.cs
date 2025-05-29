using System;

namespace EventArgs
{
    public class SendFleetArgs
    {
        public Guid DeparturePlanetId { get; set; }
        public Guid DestinationPlanetId { get; set; }
        public float Portion { get; set; }
    }
}