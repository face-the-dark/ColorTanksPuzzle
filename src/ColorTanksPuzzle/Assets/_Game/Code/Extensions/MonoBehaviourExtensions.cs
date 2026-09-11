using UnityEngine;

namespace _Game.Code.Extensions
{
    public static class MonoBehaviourExtensions
    {
        public static void StopCurrentCoroutine(this MonoBehaviour owner, ref Coroutine coroutine)
        {
            if (coroutine != null)
            {
                owner.StopCoroutine(coroutine);
                coroutine = null;
            }
        }
    }
}