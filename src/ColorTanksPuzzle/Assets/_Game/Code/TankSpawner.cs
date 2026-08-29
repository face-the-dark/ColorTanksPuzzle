using UnityEngine;
using UnityEngine.Splines;

namespace _Game.Code
{
    public class TankSpawner : MonoBehaviour
    {
        [SerializeField] private Tank _tankPrefab;
        
        private WaitingArea _waitingArea;
        private SplineContainer _spline;

        public void Initialize(WaitingArea waitingArea, SplineContainer spline)
        {
            _waitingArea = waitingArea;
            _spline = spline;
        }
        
        private void Start()
        {
            Tank tank = Instantiate(_tankPrefab);
            tank.Initialize(_spline);
            tank.CirclePassed += _waitingArea.Add;
            tank.StartMove();
        }
    }
}