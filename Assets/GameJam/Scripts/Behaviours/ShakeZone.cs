using UnityEngine;

namespace GameJam.Behaviours
{

    public class ShakeZone : MonoBehaviour
    {
        public bool playerIn { get; private set; }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                playerIn = true;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                playerIn = false;
            }
        }
    }
}
