using System;
using System.Collections.Generic;

namespace EventArgs
{
    public class GameStartedArgs
    {
        public int MapSize { get; set; }
        public string TickPeriod { get; set; }
        public float FleedSpeed { get; set; }
        public IReadOnlyDictionary<string, PlanetItem> Planets { get; set; }
        public IReadOnlyDictionary<string, PlayerItem> Players { get; set; }

        public class PlanetItem
        {
            public string Id { get; set; }
            public Location Location { get; set; }

            public int Size { get; set; }
            public string? OwnerId { get; set; }

            public int Ships { get; set; }
        }

        public class PlayerItem
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public int Color { get; set; }
        }
        
        public class Location
        {
            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}