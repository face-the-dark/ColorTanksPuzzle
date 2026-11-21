using System.Collections.Generic;
using System.Linq;
using _Game.Code.TankComponents;
using UnityEngine;

namespace _Game.Code.Spawners.Lines
{
    public class SpawnLines : MonoBehaviour
    {
        [SerializeField] private List<SpawnLine> _lines;
        
        public IReadOnlyList<SpawnLine> Lines => _lines;
        
        public int LinesCount => _lines.Count;

        public bool Contains(Tank tank) => 
            _lines.Find(spawnLine => spawnLine.Contains(tank)) != null;

        public SpawnLine Get(Tank tank) => 
            _lines.FirstOrDefault(spawnLine => spawnLine.Contains(tank));

        public void SwitchTanks(Color pixelColor)
        {
            foreach (SpawnLine spawnLine in _lines)
            {
                spawnLine.SwitchTanks(pixelColor);
            }
        }
    }
}