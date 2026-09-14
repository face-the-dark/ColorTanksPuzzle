using _Game.Code.Tanks;
using UnityEngine;

namespace _Game.Code.Players
{
    public class TankDispatcher : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private LayerMask _tankLayer;
        [SerializeField] private InputReader _inputReader;

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
                    tank.MoveToSpline();
                }
            }
        }
    }
}