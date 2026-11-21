using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Code.Extensions;
using _Game.Code.TankComponents;
using DG.Tweening;
using UnityEngine;
using VContainer;

namespace _Game.Code.Spawners.Lines
{
    public class SpawnLine : MonoBehaviour
    {
        private const float MoveDuration = 0.5f;

        [SerializeField] private float _positionOffsetZ = 5f;

        private ITankJumpAnimator _firstTankJumpAnimator;
        private ITankJumpAnimator _secondTankJumpAnimator;
        private ITankGroupMoveAnimator _tankGroupMoveAnimator;

        private readonly List<Tank> _tanks = new();
        private readonly List<SpawnLineSlot> _spawnLineSlots = new();

        private float _currentPositionOffsetZ;
        private Sequence _moveSequence;

        [Inject]
        public void Construct(ITankJumpAnimator tankJumpAnimator, ITankGroupMoveAnimator tankGroupMoveAnimator)
        {
            _firstTankJumpAnimator = tankJumpAnimator.Clone();
            _secondTankJumpAnimator = tankJumpAnimator.Clone();
            
            _tankGroupMoveAnimator = tankGroupMoveAnimator
                                     ?? throw new ArgumentNullException(nameof(tankGroupMoveAnimator));
        }

        public void Add(Tank tank)
        {
            _tanks.Add(tank);
            tank.Died += Remove;

            Vector3 slotPosition = new Vector3
            (
                transform.position.x,
                transform.position.y,
                transform.position.z - _currentPositionOffsetZ
            );

            _spawnLineSlots.Add(new SpawnLineSlot(slotPosition, tank));

            _currentPositionOffsetZ += _positionOffsetZ;
        }

        public void Remove(Tank tank)
        {
            if (_tanks.Contains(tank) && tank.IsBlocked == false)
            {
                tank.Died -= Remove;

                _tanks.Remove(tank);

                MoveTanksInLine();
            }
        }

        public bool Contains(Tank tank) =>
            _tanks.Contains(tank);

        public void UnblockFirstTank()
        {
            if (_tanks.Count > 0)
                _tanks[0].Unblock();
        }

        public void SwitchTanks(Color pixelColor)
        {
            Tank tank = _tanks.FirstOrDefault(tank => tank.Color == pixelColor);
            SpawnLineSlot spawnLineSlot = _spawnLineSlots.Find(slot => slot.EqualsTank(tank));

            if (tank != null)
            {
                SpawnLineSlot firstSpawnLineSlot = _spawnLineSlots.First();
                Tank firstTank = _tanks.First();

                firstSpawnLineSlot.SetTank(tank);
                spawnLineSlot.SetTank(firstTank);
                
                _firstTankJumpAnimator.JumpTo(firstTank.transform, spawnLineSlot.Position, 20f, MoveDuration, firstTank.Block);
                _secondTankJumpAnimator.JumpTo(tank.transform, firstSpawnLineSlot.Position, 20f, MoveDuration, tank.Unblock);

                _tanks.Swap(tank, firstTank);
            }
        }

        private void MoveTanksInLine()
        {
            List<MoveStep> steps = new List<MoveStep>();

            for (int i = 0; i < _tanks.Count; i++)
            {
                Tank currentTank = _tanks[i];
                SpawnLineSlot nextTankSlot = _spawnLineSlots[i];

                nextTankSlot.SetTank(currentTank);

                steps.Add(new MoveStep(currentTank.transform, nextTankSlot.Position));
            }

            _tankGroupMoveAnimator.MoveAll(steps, MoveDuration, UnblockFirstTank);
        }
    }
}