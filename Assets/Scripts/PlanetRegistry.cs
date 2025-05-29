using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class PlanetRegistry
{
    public List<Planet> planets = new();

    public void AddPlanet(Planet planet)
    {
        planets.Add(planet);
    }
    
    public void AddPlanets(List<Planet> planet)
    {
        planets.AddRange(planet);
    }

    public HashSet<string> GetPlanetOwners()
    {
        var hashSet = new HashSet<string>();
        foreach (var planet in planets)
        {
            hashSet.Add(planet.ownerPlayer.id);
        }
        return hashSet;
    }

    [CanBeNull]
    public Planet FindPlanetById (string id)
    {
        return planets.FirstOrDefault(planet => planet.id == id);
    }
    

    public Planet this[int index] => planets[index];

    public override string ToString()
    {
        var sb = new System.Text.StringBuilder("[");
        foreach (var planet in planets)
        {
            sb.Append(planet.ToString());
        }
        sb.Append("]");
        return sb.ToString();
    }
}
