using UnityEngine;
using UnityEngine.Splines;

namespace _Game.Code
{
    public class CompositeRoot : MonoBehaviour
    {
        [SerializeField] private WaitingAreaView _waitingAreaView;
        [SerializeField] private TankSpawner _tankSpawner;
        [SerializeField] private SplineContainer _spline;
        
        private WaitingArea _waitingArea;

        private void Awake()
        {
            _waitingArea = new WaitingArea();

            _waitingAreaView.Initialize(_waitingArea);
            _tankSpawner.Initialize(_waitingArea, _spline);
        }
    }
}