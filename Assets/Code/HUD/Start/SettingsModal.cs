using Code.Audio;
using Code.Main;
using Code.Saves;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace Code.HUD.Start
{
    public class SettingsModal : MonoBehaviour
    {
        [SerializeField] private string _legalURL;
        
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _soundSlider;
        [SerializeField] private Button _legalDocsButton;
        [SerializeField] private Button _accountTerminationButton;
        [SerializeField] private OverlayClose _overlayClose;
        
        
        
        private AudioManager _audioManager;
        private PlayerSettings _settings;


        public void Init(PlayerSettings settings, AudioManager audioManager)
        {
            _settings = settings;
            _audioManager = audioManager;
            
            Hide();
        }


        private void OnEnable()
        {
            if (_settings == null) return;
            _musicSlider.value = _settings.GetMusicVolume();
            _soundSlider.value = _settings.GetSoundVolume();
            _musicSlider.onValueChanged.AddListener(OnMusicChanged);
            _soundSlider.onValueChanged.AddListener(OnSoundChanged);
            _accountTerminationButton.onClick.AddListener(OnAccountTerminationClicked);
            _legalDocsButton.onClick.AddListener(OnLegalDocsClicked);
            _overlayClose.Triggered += Hide;
        }

        private void OnLegalDocsClicked()
        {
            Application.OpenURL(_legalURL);
        }

        private void OnAccountTerminationClicked()
        {
            MainEntryPoint.Instance.Profile.Clear();
            MainEntryPoint.Instance.Settings.Clear();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            _musicSlider.onValueChanged.RemoveAllListeners();
            _soundSlider.onValueChanged.RemoveAllListeners();
            _accountTerminationButton.onClick.RemoveAllListeners();
            _legalDocsButton.onClick.RemoveAllListeners();
            _overlayClose.Triggered -= Hide;
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