using System;
using _Game.Code.Providers;
using _Game.Code.Tanks;
using UnityEngine;
using VContainer;

namespace _Game.Code.Players
{
    public class TankDispatcher : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private Camera _camera;

        [Header("Settings")]
        [SerializeField] private LayerMask _tankLayer;
        
        private InputReader _inputReader;

        private int _maxTanksCountOnSpline;
        private int _currentTanksCountOnSpline;
        
        public event Action<string> TanksCountChanged;

        [Inject]
        public void Construct(InputReader inputReader, LevelConfigurationProvider levelConfigurationProvider)
        {
            _inputReader = inputReader ?? throw new ArgumentNullException(nameof(inputReader));
            
            if (levelConfigurationProvider is null)
                throw new ArgumentNullException(nameof(levelConfigurationProvider));
            
            _maxTanksCountOnSpline = levelConfigurationProvider.GetDifficultyConfiguration().StartMaxTanksCount;
        }

        private void Start() => 
            TanksCountChanged?.Invoke($"{_currentTanksCountOnSpline}/{_maxTanksCountOnSpline}");

        private void OnEnable() => 
            _inputReader.Clicked += OnClicked;

        private void OnDisable() => 
            _inputReader.Clicked -= OnClicked;

        private void OnClicked(Vector2 position)
        {
            Ray ray = _camera.ScreenPointToRay(position);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _tankLayer))
            {
                if (hit.collider.TryGetComponent(out Tank tank) && tank.IsMoving == false && tank.IsBlocked == false)
                {
                    if (_currentTanksCountOnSpline < _maxTanksCountOnSpline)
                    {
                        tank.MoveToSpline();
                        tank.MovingStopped += OnTankMovingStopped;
                        _currentTanksCountOnSpline++;
                        
                        TanksCountChanged?.Invoke($"{_currentTanksCountOnSpline}/{_maxTanksCountOnSpline}");
                    }
                }
            }
        }

        private void OnTankMovingStopped(Tank tank)
        {
            if (tank is null)
                throw new ArgumentNullException(nameof(tank));
            
            _currentTanksCountOnSpline--;
            tank.MovingStopped -= OnTankMovingStopped;
            
            TanksCountChanged?.Invoke($"{_currentTanksCountOnSpline}/{_maxTanksCountOnSpline}");
        }

        public void IncreaseMaxCellsCount()
        {
            _maxTanksCountOnSpline++;
            
            TanksCountChanged?.Invoke($"{_currentTanksCountOnSpline}/{_maxTanksCountOnSpline}");
        }
    }
}