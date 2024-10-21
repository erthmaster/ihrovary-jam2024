using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace GameJam.UI
{

    public class Settings : MonoBehaviour
    {
        [SerializeField] private AudioMixerGroup _mixer;
        [SerializeField] private Toggle _musicToggle;
        [SerializeField] private Toggle _effectsToggle;
        public void ToggleMusic()
        {
            _musicToggle.animator.SetBool("On",_musicToggle.isOn);
            if (_musicToggle.isOn)
                _mixer.audioMixer.SetFloat("MusicVolume", 0);
            else
                _mixer.audioMixer.SetFloat("MusicVolume", -80);
        }
        public void ToggleEffects()
        {
            _effectsToggle.animator.SetBool("On", _effectsToggle.isOn);
            if (_effectsToggle.isOn)
                _mixer.audioMixer.SetFloat("EffectsVolume", 0);
            else
                _mixer.audioMixer.SetFloat("EffectsVolume", -80);
        }
    }
}