using VContainer;

namespace _Game.Code.Tanks
{
    public class Reload
    {
        private const float DistancePerShot = 1f;  
        
        private float _distanceSinceLastShot;
        private float _previousDistance;
        private bool _initialized;

        public bool CanShoot { get; private set; } = true;

        public void UpdateDistance(float currentDistance)
        {
            if (!_initialized)
            {
                _previousDistance = currentDistance;
                _initialized = true;
                return;
            }

            float delta = currentDistance - _previousDistance;
            _previousDistance = currentDistance;

            if (delta > 0f)
            {
                _distanceSinceLastShot += delta;
            }

            CanShoot = _distanceSinceLastShot >= DistancePerShot;
        }

        public void StartReload()
        {
            if (!CanShoot)
                return;

            _distanceSinceLastShot -= DistancePerShot;
            CanShoot = _distanceSinceLastShot >= DistancePerShot;
        }
    }
}