using System;

namespace EventArgs
{
    public class FleetSentArgs
    {
        public  FleetItem Fleet { get; set; }
        public  DeparturePlanetItem DeparturePlanet { get; set; }
        public  DestinationPlanetItem DestinationPlanet { get; set; }

        public class FleetItem
        {
            public  Guid Id { get; set; }
            public  Guid OwnerId { get; set; }
            public  int Ships { get; set; }
        }

        public class DeparturePlanetItem
        {
            public  Guid Id { get; set; }
            public  int Ships { get; set; }
        }

        public class DestinationPlanetItem
        {
            public  Guid Id { get; set; }
        }
    }
}