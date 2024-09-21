using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace GameJam
{
    public class MusicChangeManager : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource1;
        [SerializeField] private AudioSource _audioSource2;
        [SerializeField] private AudioClip[] _musicPatterns;

        private int currentPatternIndex;
        public int currentIndexNeed;

        [SerializeField] private float _earlyStartTime;

        private bool isPlayingFirstSource = true;

        void Start()
        {
            StartCoroutine(ChangeMusicPattern());
        }

        IEnumerator ChangeMusicPattern()
        {
            while (true)
            {
                AudioSource activeSource = isPlayingFirstSource ? _audioSource1 : _audioSource2;
                AudioSource nextSource = isPlayingFirstSource ? _audioSource2 : _audioSource1;

                activeSource.clip = _musicPatterns[currentPatternIndex];
                activeSource.Play();

                yield return new WaitForSecondsRealtime(_earlyStartTime);
                if (currentIndexNeed > currentPatternIndex)
                {
                    nextSource.clip = _musicPatterns[(currentPatternIndex + 1) % _musicPatterns.Length];
                    currentPatternIndex = (currentPatternIndex + 1) % _musicPatterns.Length;
                }
                nextSource.Play();

                isPlayingFirstSource = !isPlayingFirstSource;
            }
        }
    }
}