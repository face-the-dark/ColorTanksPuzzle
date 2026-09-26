using System;
using System.Collections.Generic;
using _Game.Code.Providers;
using _Game.Code.Tanks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace _Game.Code.Players
{
    public class TankDispatcher : IDisposable
    {
        private readonly LevelConfigurationProvider _levelConfigurationProvider;
        private readonly BonusesConfigurationProvider _bonusesConfigurationProvider;
        private readonly TankSelector _tankSelector;
        private readonly List<Tank> _tanksOnSpline = new();

        private int _maxTanksCountOnSpline;
        private Coroutine _unfreezeTanksCoroutine;

        private bool _isBlocked;

        public event Action<string> TanksCountChanged;

        [Inject]
        public TankDispatcher
        (
            LevelConfigurationProvider levelConfigurationProvider,
            BonusesConfigurationProvider bonusesConfigurationProvider,
            TankSelector tankSelector
        )
        {
            _bonusesConfigurationProvider = bonusesConfigurationProvider ??
                                            throw new ArgumentNullException(nameof(bonusesConfigurationProvider));

            _levelConfigurationProvider = levelConfigurationProvider ??
                                          throw new ArgumentNullException(nameof(levelConfigurationProvider));
            
            _tankSelector = tankSelector ?? throw new ArgumentNullException(nameof(tankSelector));

            _tankSelector.TankSelected += Dispatch;

            Initialize();
        }

        public void Dispose()
        {
            _tankSelector.TankSelected -= Dispatch;
        }

        public void FreezeTanksOnSpline()
        {
            foreach (Tank tank in _tanksOnSpline)
                tank.FreezeMoving();

            DelayUnfreezeTanks().Forget();
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

        private void Initialize()
        {
            _maxTanksCountOnSpline = _levelConfigurationProvider.GetDifficultyConfiguration().StartMaxTanksCount;

            TanksCountChanged?.Invoke($"{_tanksOnSpline.Count}/{_maxTanksCountOnSpline}");
        }

        private async UniTask DelayUnfreezeTanks()
        {
            await UniTask.WaitForSeconds((int)_bonusesConfigurationProvider.BonusesConfiguration.FreezeSplineBonusTime);

            foreach (Tank tank in _tanksOnSpline)
                tank.UnfreezeMoving();
        }

        private void Dispatch(Tank tank)
        {
            if (_isBlocked == false)
            {
                if (_tanksOnSpline.Count < _maxTanksCountOnSpline && tank.IsBlocked == false)
                {
                    tank.MoveToSpline();
                    tank.MovingStopped += OnTankMovingStopped;

                    _tanksOnSpline.Add(tank);

                    TanksCountChanged?.Invoke($"{_tanksOnSpline.Count}/{_maxTanksCountOnSpline}");
                }
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
    }
}