using System.Collections.Generic;
using _Game.Code.Tanks;
using UnityEngine;

namespace _Game.Code.Spawners
{
    public class SpawningLane : MonoBehaviour
    {
        [SerializeField] private float _positionOffsetZ = 4f;

        private readonly List<Tank> _tanks = new();

        private float _currentPositionOffsetZ;

        public void Add(Tank tank)
        {
            _tanks.Add(tank);

            tank.transform.position = new Vector3
            (
                transform.position.x,
                transform.position.y,
                transform.position.z - _currentPositionOffsetZ
            );

            _currentPositionOffsetZ += _positionOffsetZ;
        }

        public void Remove(Tank tank)
        {
            if (_tanks.Contains(tank) && tank.IsBlocked == false)
            {
                for (int i = _tanks.Count - 1; i > 0; i--)
                {
                    _tanks[i].transform.position = _tanks[i - 1].transform.position;
                }

                _tanks.Remove(tank);

                UnblockFirstTank();
            }
        }

        public void UnblockFirstTank()
        {
            if (_tanks.Count > 0)
                _tanks[0].Unblock();
        }
    }
}