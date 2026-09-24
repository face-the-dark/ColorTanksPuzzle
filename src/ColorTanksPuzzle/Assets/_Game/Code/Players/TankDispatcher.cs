using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Code.Extensions;
using _Game.Code.Providers;
using _Game.Code.Tanks;
using UnityEngine;
using VContainer;

namespace _Game.Code.Players
{
    public class TankDispatcher : MonoBehaviour
    {
        [Header("Dependencies")] [SerializeField]
        private Camera _camera;

        [Header("Settings")] [SerializeField] private LayerMask _tankLayer;

        private InputReader _inputReader;
        private BonusesConfigurationProvider _bonusesConfigurationProvider;

        private List<Tank> _tanksOnSpline = new();
        private int _maxTanksCountOnSpline;
        private Coroutine _unfreezeTanksCoroutine;

        public event Action<string> TanksCountChanged;

        [Inject]
        public void Construct
        (
            InputReader inputReader,
            LevelConfigurationProvider levelConfigurationProvider,
            BonusesConfigurationProvider bonusesConfigurationProvider
        )
        {
            _inputReader = inputReader ?? throw new ArgumentNullException(nameof(inputReader));
            _bonusesConfigurationProvider = bonusesConfigurationProvider ??
                                            throw new ArgumentNullException(nameof(bonusesConfigurationProvider));

            if (levelConfigurationProvider is null)
                throw new ArgumentNullException(nameof(levelConfigurationProvider));

            _maxTanksCountOnSpline = levelConfigurationProvider.GetDifficultyConfiguration().StartMaxTanksCount;
        }

        private void Start() =>
            TanksCountChanged?.Invoke($"{_tanksOnSpline.Count}/{_maxTanksCountOnSpline}");

        private void OnEnable() =>
            _inputReader.Clicked += OnClicked;

        private void OnDisable() =>
            _inputReader.Clicked -= OnClicked;

        public void FreezeTanks()
        {
            foreach (Tank tank in _tanksOnSpline)
                tank.FreezeMoving();

            this.StopCurrentCoroutine(ref _unfreezeTanksCoroutine);
            _unfreezeTanksCoroutine = StartCoroutine(DelayUnfreezeTanks());
        }

        private IEnumerator DelayUnfreezeTanks()
        {
            yield return new WaitForSeconds(_bonusesConfigurationProvider.BonusesConfiguration.FreezeSplineBonusTime);

            foreach (Tank tank in _tanksOnSpline)
                tank.UnfreezeMoving();
        }

        private void OnClicked(Vector2 position)
        {
            Ray ray = _camera.ScreenPointToRay(position);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _tankLayer))
            {
                if (hit.collider.TryGetComponent(out Tank tank) && tank.IsMoving == false && tank.IsBlocked == false)
                {
                    if (_tanksOnSpline.Count < _maxTanksCountOnSpline)
                    {
                        tank.MoveToSpline();
                        tank.MovingStopped += OnTankMovingStopped;

                        _tanksOnSpline.Add(tank);

                        TanksCountChanged?.Invoke($"{_tanksOnSpline.Count}/{_maxTanksCountOnSpline}");
                    }
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

        public void IncreaseMaxCellsCount()
        {
            _maxTanksCountOnSpline++;

            TanksCountChanged?.Invoke($"{_tanksOnSpline.Count}/{_maxTanksCountOnSpline}");
        }
    }
}