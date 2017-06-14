using UnityEngine;

namespace _2SpaceShooter
{
    public class Done_DestoryByBoundary : MonoBehaviour
    {
        private void OnTriggerExit(Collider other)
        {
            Destroy(other.gameObject);
        }
    }
}