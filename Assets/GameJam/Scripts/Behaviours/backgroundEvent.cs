using UnityEngine;

namespace GameJam.Behaviours
{
    public class backgroundEvent : MonoBehaviour
    {
        [SerializeField] private Item _item;

        public void AnimEnd()
        {
            _item.Fade();

        }
        public void SpawnParticles()
        {
            if (TryGetComponent(out ParticleSystem s))
            {
                s.Play();
            }
        }
    }
}