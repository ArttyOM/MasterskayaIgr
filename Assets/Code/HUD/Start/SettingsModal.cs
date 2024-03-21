using Code.Audio;
using Code.Saves;
using UnityEngine;
using UnityEngine.UI;

namespace Code.HUD.Start
{
    public class SettingsModal : MonoBehaviour
    {
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _soundSlider;
        
        private AudioManager _audioManager;
        private PlayerSettings _settings;


        public void Init(PlayerSettings settings, AudioManager audioManager)
        {
            _settings = settings;
            _audioManager = audioManager;
        }

        private void OnEnable()
        {
            if (_settings == null) return;
            _musicSlider.value = _settings.GetMusicVolume();
            _soundSlider.value = _settings.GetSoundVolume();
            _musicSlider.onValueChanged.AddListener(OnMusicChanged);
            _soundSlider.onValueChanged.AddListener(OnSoundChanged);
        }

        private void OnDisable()
        {
            _musicSlider.onValueChanged.RemoveAllListeners();
            _soundSlider.onValueChanged.RemoveAllListeners();
        }

        private void OnSoundChanged(float newValue)
        {
            _settings.SetSoundVolume(newValue);
            _audioManager.SetSoundVolumeFromNormalized(newValue);
        }

        private void OnMusicChanged(float newValue)
        {
            _settings.SetMusicVolume(newValue);
            _audioManager.SetMusicVolumeFromNormalized(newValue);
        }
    }
}