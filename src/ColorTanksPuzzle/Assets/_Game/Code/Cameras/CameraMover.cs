using System;
using DG.Tweening;
using UnityEngine;
using VContainer;

namespace _Game.Code.Cameras
{
    public class CameraMover
    {
        private readonly Camera _camera;
        private readonly Vector3 _tanksPoint;
        private readonly Vector3 _artPoint;
        private readonly Vector3 _startingPoint;

        [Inject]
        public CameraMover(Camera camera, ZoomPoints zoomPoints)
        {
            _camera = camera ?? throw new ArgumentNullException(nameof(camera));
            
            if (zoomPoints is null)
                throw new ArgumentNullException(nameof(zoomPoints));

            _tanksPoint = zoomPoints.CameraTanksPoint.position;
            _artPoint = zoomPoints.CameraArtPoint.position;
            _startingPoint = camera.transform.position;
        }

        public void ZoomToStart() => 
            _camera.transform.DOMove(_startingPoint, 1f);

        public void ZoomToTanks() => 
            _camera.transform.DOMove(_tanksPoint, 1f);

        public void ZoomToArt() => 
            _camera.transform.DOMove(_artPoint, 1f);
    }
}