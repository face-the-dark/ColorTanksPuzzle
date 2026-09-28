using UnityEngine;

namespace _Game.Code.Cameras
{
    public class ZoomPoints : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTanksPoint;
        [SerializeField] private Transform _cameraArtPoint;
        
        public Transform CameraTanksPoint => _cameraTanksPoint;
        public Transform CameraArtPoint => _cameraArtPoint;
    }
}