using System.Collections;
using UnityEngine;

namespace _Game.Code
{
    public class WaitingAreaCircle : MonoBehaviour
    {
        [SerializeField] private WaitingArea _waitingArea;
        [SerializeField] private TankSpawner _tankSpawner;

        private void OnEnable()
        {
            _tankSpawner.TankSpawned += OnTankSpawned;
        }

        private void OnDisable()
        {
            _tankSpawner.TankSpawned -= OnTankSpawned;
        }

        private void OnTankSpawned(Tank tank)
        {
            tank.CirclePassed += OnCirclePassed;
            tank.StartMove();
        }

        private void OnCirclePassed(Tank tank)
        {
            tank.CirclePassed -= OnCirclePassed;
            _waitingArea.Add(tank);
            StartCoroutine(Delay(tank));
        }

        private IEnumerator Delay(Tank tank)
        {
            yield return new WaitForSeconds(1f);

            StubMove(tank);
        }

        private void StubMove(Tank tank)
        {
            _waitingArea.Remove(tank);
            tank.CirclePassed += OnCirclePassed;
            tank.StartMove();
        }
    }
}