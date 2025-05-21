using System.Collections.Generic;
using System.Threading.Tasks;

public interface IPlanetSpawner
{
    public Task<List<Planet>> SpawnPlanets();
}
