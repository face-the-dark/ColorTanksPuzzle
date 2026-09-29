using System.Collections.Generic;
using UnityEngine;

namespace _Game.Code.Spawners
{
    public class SpawningLanesContainer : MonoBehaviour
    {
        [SerializeField] private List<SpawningLane> _spawningLanes;
        
        public IReadOnlyList<SpawningLane> SpawningLanes => _spawningLanes;
        
        public int LanesCount => _spawningLanes.Count;
    }
}