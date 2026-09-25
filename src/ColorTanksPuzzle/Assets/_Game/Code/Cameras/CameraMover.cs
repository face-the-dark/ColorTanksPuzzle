using System;
using DG.Tweening;
using UnityEngine;
using VContainer;

namespace _Game.Code.Cameras
{
    public class CameraMover
    {
        private readonly Camera _camera;
        private readonly Vector3 _separationPoint;
        private readonly Vector3 _startingPoint;

        [Inject]
        public CameraMover(Camera camera, Transform separationPoint)
        {
            _camera = camera ?? throw new ArgumentNullException(nameof(camera));
            
            if (separationPoint is null)
                throw new ArgumentNullException(nameof(separationPoint));
            
            _separationPoint = separationPoint.position;
            _startingPoint = camera.transform.position;
        }

        public void Zoom()
        {
            _camera.transform.DOMove(_startingPoint, 1f);
        }

        public void Unzoom()
        {
            _camera.transform.DOMove(_separationPoint, 1f);
        }
    }
}