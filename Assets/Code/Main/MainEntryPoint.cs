using System;
using System.Linq;
using Code.Audio;
using Code.Events;
using Code.GameLoop;
using Code.HUD;
using Code.HUD.LevelSelect;
using Code.HUD.Offers;
using Code.HUD.ScreenActivators;
using Code.HUD.Start;
using Code.Levels;
using Code.Pools;
using Code.Saves;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Code.Main
{
    [DisallowMultipleComponent]
    public class MainEntryPoint : MonoBehaviour
    {
        [field: SerializeField] public PoolCommonParent PoolCommonParent { get; private set; }
        [SerializeField] private LevelProgression _levelProgression;
        [SerializeField] private LevelSelectScreen _levelSelect;
        [SerializeField] private StartScreen _startScreen;
        [SerializeField] private SettingsModal _settingsModal;
        [SerializeField] private OffersManager _offersManager;
        
        [SerializeField] private AudioManager _audioManager;
        
        private ScreenSwitcher _screenSwitcher;
        private InGameEvents _events;
        private LevelLoader _levelLoader;
        private IPersistentStorage _storage;
        private PlayerProfile _profile;
        private LevelSelectionScreenActivator _levelSelectionScreenActivator;
        private PlayerSettings _settings;

        private async void Awake()
        {
            _events = new InGameEvents();
            _storage = new PlayerPrefsStorage();
            _profile = new PlayerProfile(_storage);
            _screenSwitcher = new ScreenSwitcher();
            _settings = new PlayerSettings(_storage);
            _levelSelect.Init(_levelProgression, _events, _screenSwitcher);
            _settingsModal.Init(_settings, _audioManager);
            _startScreen.Init(_events, _screenSwitcher, _settingsModal, _offersManager);
            
            _levelLoader = new LevelLoader(_screenSwitcher, _events);
            _events.OnLevelStart.Subscribe(levelIndex => _levelLoader.LoadLevelWithSceneIndex(levelIndex).Forget());
            if (_profile.IsFirstLaunch())
            {
                var defaultLevel = _levelProgression.DefaultLevel;
                await _levelLoader.LoadLevelWithSceneIndex(defaultLevel.BuildIndex);
            }
                
            _profile.IncrementLaunchCount();
        }

        private void Start()
        {
            _audioManager.SetMusicVolumeFromNormalized(_settings.GetMusicVolume());
            _audioManager.SetSoundVolumeFromNormalized(_settings.GetSoundVolume());
        }
    }
}