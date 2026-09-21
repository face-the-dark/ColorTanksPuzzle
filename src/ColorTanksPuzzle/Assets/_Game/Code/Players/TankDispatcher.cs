using System;
using _Game.Code.Tanks;
using UnityEngine;

namespace _Game.Code.Players
{
    public class TankDispatcher : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private Camera _camera;
        [SerializeField] private InputReader _inputReader;
        
        [Header("Settings")]
        [SerializeField] private LayerMask _tankLayer;
        [SerializeField] private int _maxTanksCountOnSpline = 5;
        
        private int _currentTanksCountOnSpline;
        
        public event Action<string> TanksCountChanged;

        private void Start()
        {
            TanksCountChanged?.Invoke($"{_currentTanksCountOnSpline}/{_maxTanksCountOnSpline}");
        }

        private void OnEnable()
        {
            _inputReader.Clicked += OnClicked;
        }

        private void OnDisable()
        {
            _inputReader.Clicked -= OnClicked;
        }

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
            _currentTanksCountOnSpline--;
            tank.MovingStopped -= OnTankMovingStopped;
            
            TanksCountChanged?.Invoke($"{_currentTanksCountOnSpline}/{_maxTanksCountOnSpline}");
        }
    }
}