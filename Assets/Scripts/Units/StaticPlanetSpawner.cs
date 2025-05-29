using System.Collections.Generic;
using System.Threading.Tasks;
using EventArgs;
using UnityEngine;

namespace Units
{
    public class StaticPlanetSpawner : PlanetSpawnerBase
    {
        public GameObject planetPrefab;
        public Dictionary<string, Player> players = new(6);
        private List<Color> _colors = new() { Color.red, Color.green, Color.blue, Color.yellow, Color.magenta, Color.cyan };
        public override Task<List<Planet>> SpawnPlanets()
        {
            var planets = new List<Planet>();
            CreatePlayers();
            foreach (var planetObj in RoomInfo.Planets)
            {
                var planet = CreatePlanet(planetObj.Value);
                planets.Add(planet);
            }

            return Task.FromResult(planets);
        }

        private void CreatePlayers ()
        {
            foreach (var playerObj in RoomInfo.Players)
            {
                players[playerObj.Key] = new Player(playerObj.Value.Name, playerObj.Value.Id, _colors[playerObj.Value.Color]);
            }
        }

        private Planet CreatePlanet(GameStartedArgs.PlanetItem planetDto)
        {
            var planetGameObj = Instantiate(planetPrefab);
            var planet = planetGameObj.GetComponent<Planet>();
            var spriteRenderer = planetGameObj.GetComponent<SpriteRenderer>();
            planet.id = planetDto.Id;
            planet.transform.position = new Vector3((float) planetDto.Location.X, (float) planetDto.Location.Y, 0);
            planet.ownerPlayer = planetDto.OwnerId is not null && players.TryGetValue(planetDto.OwnerId, out var player) ? player : Player.NoOne();
            planet.SetSize(planetDto.Size);
            planet.shipsAmount = planetDto.Ships;
            spriteRenderer.color = planet.ownerPlayer.color;
            return planet;
        }
    }
}