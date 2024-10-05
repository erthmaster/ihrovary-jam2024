using UnityEngine;

namespace GameJam
{
    public class PlayerAudioManager : MonoBehaviour
    {
        [SerializeField] private AudioClip[] _clips;
        [SerializeField] private AudioClip _hitClip;
        [SerializeField] private AudioSource _audioSource;
        public void Walk()
        {
            _audioSource.pitch = Random.Range(1.15f, 0.95f);
            _audioSource.clip = _clips[Random.Range(0, _clips.Length)];
            _audioSource.Play();
        }
        public void Hit()
        {
            _audioSource.pitch = Random.Range(1.15f, 0.95f);
            _audioSource.clip = _hitClip;
            _audioSource.Play();
        }
    }
}
