using Code.Audio;
using Code.DebugTools.Logger;
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
using UniRx;
using UnityEngine;


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
        public static MainEntryPoint Instance { get; private set; }= null;
        public InGameEvents Events => _events;
        public ScreenSwitcher ScreenSwitcher => _screenSwitcher;
        public AudioManager AudioManager => _audioManager;
        public PlayerSettings Settings => _settings;
        public PlayerProfile Profile => _profile;
        public LevelProgression LevelProgression => _levelProgression;
        public LevelLoader LevelLoader => _levelLoader;

        private void Awake()
        {
            if (Instance != null)
            {
                "There already are MainEntryPoint, duplicate".LogWarning(gameObject);
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            
            _events = new InGameEvents();
            _storage = new PlayerPrefsStorage();
            _profile = new PlayerProfile(_storage);
            _screenSwitcher = new ScreenSwitcher();
            _settings = new PlayerSettings(_storage);
            _levelSelect.Init(_levelProgression, _events, _screenSwitcher);
            _settingsModal.Init(_settings, _audioManager);
            _startScreen.Init(_events, _screenSwitcher, _settingsModal, _offersManager);
            
            _levelLoader = new LevelLoader(_events);
            _events.OnLevelStart.Subscribe(StartLevel);
            Instance = this;
        }

        private async void StartLevel(int levelIndex) => await _levelLoader.LoadLevelWithSceneIndex(levelIndex);
    }
}