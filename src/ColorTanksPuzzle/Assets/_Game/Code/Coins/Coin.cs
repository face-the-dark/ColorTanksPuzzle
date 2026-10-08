using System.Collections;
using _Game.Code.Extensions;
using UnityEngine;

namespace _Game.Code.Coins
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] private float _duration = 0.8f;
        [SerializeField] private float _arcHeight = 1.5f;

        private Camera _camera;

        private Coroutine _flyCoroutine;

        public void Construct(Camera camera, RectTransform coinUiTarget)
        {
            _camera = camera;

            StartFly(coinUiTarget);
        }

        private void StartFly(RectTransform coinUiTarget)
        {
            this.StopCurrentCoroutine(ref _flyCoroutine);
            _flyCoroutine = StartCoroutine(Fly(coinUiTarget));
        }

        private IEnumerator Fly(RectTransform coinUiTarget)
        {
            Vector3 startPosition = transform.position;
            Vector3 middlePosition = startPosition + Vector3.up * _arcHeight;

            float time = 0f;

            while (time < _duration)
            {
                time += Time.deltaTime;

                float step = time / _duration;

                Vector3 screenTarget = RectTransformUtility.WorldToScreenPoint(null, coinUiTarget.position);

                float distance = Mathf.Abs(_camera.transform.position.z);

                Vector3 target = _camera.ScreenToWorldPoint(new Vector3(screenTarget.x, screenTarget.y, distance));
                
                Vector3 position = Mathf.Pow(1 - step, 2) * startPosition +
                                   2 * (1 - step) * step * middlePosition +
                                   Mathf.Pow(step, 2) * target;

                transform.position = position;

                yield return null;
            }

            Destroy(gameObject);
        }
    }
}