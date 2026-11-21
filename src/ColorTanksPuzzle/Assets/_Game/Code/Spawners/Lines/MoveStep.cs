using UnityEngine;

namespace _Game.Code.Spawners.Lines
{
    public readonly struct MoveStep
    {
        public readonly Transform Target;
        public readonly Vector3 Destination;

        public MoveStep(Transform target, Vector3 destination)
        {
            Target = target;
            Destination = destination;
        }
    }
}