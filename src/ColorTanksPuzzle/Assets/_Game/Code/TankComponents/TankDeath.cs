using UnityEngine;

namespace _Game.Code.TankComponents
{
    public class TankDeath : MonoBehaviour
    {
        public void Die()
        {
            Destroy(gameObject);
        }
    }
}