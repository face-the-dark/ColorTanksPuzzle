using System;
using System.Collections.Generic;
using _Game.Code.ConfigurationProvider;
using _Game.Code.Spawners.Lines;
using _Game.Code.TankComponents;
using _Game.Code.WaitingAreaComponents;
using UnityEngine;
using VContainer;

namespace _Game.Code.Players
{
    public class TankDispatcher : IDisposable
    {
        private readonly LevelConfigurationProvider _levelConfigurationProvider;
        private readonly Selector _selector;
        private readonly WaitingArea _waitingArea;
        private readonly SpawnLines _spawnLines;
        private readonly List<Tank> _tanksOnSpline = new();

        private int _maxTanksCountOnSpline;
        private Coroutine _unfreezeTanksCoroutine;

        private bool _isBlocked;

        public event Action<string> TanksCountChanged;

        [Inject]
        public TankDispatcher
        (
            LevelConfigurationProvider levelConfigurationProvider,
            Selector selector,
            WaitingArea waitingArea,
            SpawnLines spawnLines
        )
        {
            _levelConfigurationProvider = levelConfigurationProvider ??
                                          throw new ArgumentNullException(nameof(levelConfigurationProvider));

            _selector = selector ?? throw new ArgumentNullException(nameof(selector));
            _waitingArea = waitingArea ?? throw new ArgumentNullException(nameof(waitingArea));
            _spawnLines = spawnLines ?? throw new ArgumentNullException(nameof(spawnLines));

            _selector.TankSelected += Dispatch;
        }

        public void Dispose()
        {
            _selector.TankSelected -= Dispatch;
        }

        public void IncreaseMaxCellsCount()
        {
            _maxTanksCountOnSpline++;

            TanksCountChanged?.Invoke($"{_tanksOnSpline.Count}/{_maxTanksCountOnSpline}");
        }

        public void Block() =>
            _isBlocked = true;

        public void Unblock() =>
            _isBlocked = false;

        public void Initialize()
        {
            _maxTanksCountOnSpline = _levelConfigurationProvider.GetDifficultyConfiguration().StartMaxTanksCount;

            TanksCountChanged?.Invoke($"{_tanksOnSpline.Count}/{_maxTanksCountOnSpline}");
        }

        private void Dispatch(Tank tank)
        {
            if (_isBlocked == false)
            {
                if (_tanksOnSpline.Count < _maxTanksCountOnSpline && tank.IsBlocked == false)
                {
                    RemoveFromWaitingAreaOrLane(tank);

                    tank.MoveToSpline();
                    tank.CirclePassed += OnCirclePassed;
                    tank.MovingStopped += OnTankMovingStopped;
                    tank.Died += OnTankDied;

                    _tanksOnSpline.Add(tank);

                    TanksCountChanged?.Invoke($"{_tanksOnSpline.Count}/{_maxTanksCountOnSpline}");
                }
            }
        }

        private void OnTankDied(Tank tank)
        {
            if (tank is null)
                throw new ArgumentNullException(nameof(tank));

            RemoveFromWaitingAreaOrLane(tank);

            tank.CirclePassed -= OnCirclePassed;
            tank.MovingStopped -= OnTankMovingStopped;
            tank.Died -= OnTankDied;
        }

        private void RemoveFromWaitingAreaOrLane(Tank tank)
        {
            if (tank is null)
                throw new ArgumentNullException(nameof(tank));

            if (_waitingArea.Contains(tank))
            {
                _waitingArea.Remove(tank);
            }
            else if (_spawnLines.Contains(tank))
            {
                SpawnLine spawnLine = _spawnLines.Get(tank);

                if (spawnLine is null)
                    throw new InvalidOperationException(nameof(spawnLine));

                spawnLine.Remove(tank);
            }
        }

        private void OnTankMovingStopped(Tank tank)
        {
            if (tank is null)
                throw new ArgumentNullException(nameof(tank));

            tank.MovingStopped -= OnTankMovingStopped;
            
            _tanksOnSpline.Remove(tank);

            TanksCountChanged?.Invoke($"{_tanksOnSpline.Count}/{_maxTanksCountOnSpline}");
        }

        private void OnCirclePassed(Tank tank)
        {
            if (tank is null)
                throw new ArgumentNullException(nameof(tank));

            tank.CirclePassed -= OnCirclePassed;

            _waitingArea.Add(tank);
        }
    }
}