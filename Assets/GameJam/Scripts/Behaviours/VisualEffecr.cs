using UnityEngine;

namespace GameJam
{
    public class VisualEffecr : MonoBehaviour
    {
        [SerializeField] private float lifetime;

        private void Start()
        {
            Invoke(nameof(Destroy), lifetime);
        }
        private void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
