using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace GameJam.UI
{

    public class Settings : MonoBehaviour
    {
        [SerializeField] private AudioMixerGroup _mixer;
        [SerializeField] private Slider _globalSlider;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _effectsSlider;
        public void ChangeGlobalVolume()
        {
            _mixer.audioMixer.SetFloat("GlobalVolume", _globalSlider.value);
        }
        public void ChangeMusicVolume()
        {
            _mixer.audioMixer.SetFloat("MusicVolume", _musicSlider.value);
        }
        public void ChangeEffectsVolume()
        {
            _mixer.audioMixer.SetFloat("EffectsVolume", _effectsSlider.value);
        }
    }
}