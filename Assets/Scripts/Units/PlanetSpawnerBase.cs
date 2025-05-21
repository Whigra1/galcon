using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlanetSpawnerBase : MonoBehaviour, IPlanetSpawner
{
    public virtual Task<List<Planet>> SpawnPlanets()
    {
        throw new System.NotImplementedException();
    }

}
