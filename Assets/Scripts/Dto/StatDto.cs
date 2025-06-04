using UnityEngine;

namespace Dto
{
    [System.Serializable]
    public class StatsDto
    {
        [SerializeField] public long totalGames;
        [SerializeField] public long wonGames;
    }
}