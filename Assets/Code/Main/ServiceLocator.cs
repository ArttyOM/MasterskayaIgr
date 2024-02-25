using Code.Audio;
using Code.DebugTools.Logger;
using Code.Events;
using Code.GameLoop;
using Code.HUD;
using Code.HUD.LevelSelect;
using Code.HUD.Offers;
using Code.HUD.Start;
using Code.Levels;
using Code.PregameShop;
using Code.Saves;
using Code.Spells;
using Code.Upgrades;
using UniRx;
using UnityEngine;


namespace Code.Main
{
    [DisallowMultipleComponent]
    public class ServiceLocator : MonoBehaviour
    {
        [SerializeField] private LevelProgression _levelProgression;
        [SerializeField] private LevelSelectScreen _levelSelect;
        [SerializeField] private StartScreen _startScreen;
        [SerializeField] private SettingsModal _settingsModal;
        [SerializeField] private OffersManager _offersManager;
        [SerializeField] private SpellShop _spellShop;
        [SerializeField] private UpgradeList _upgrades;
        [SerializeField] private AudioManager _audioManager;
        [SerializeField] private Canvas _dragCanvas;
        
        
        private ScreenSwitcher _screenSwitcher;
        private InGameEvents _events;
        private LevelLoader _levelLoader;
        private IPersistentStorage _storage;
        private PlayerProfile _profile;
        private PlayerSettings _settings;
        private ShopSystem _shopSystem;
        private UpgradeSystem _upgradeSystem;
        
        public static ServiceLocator Instance { get; private set; }= null;
        public InGameEvents Events => _events;
        public ScreenSwitcher ScreenSwitcher => _screenSwitcher;
        public AudioManager AudioManager => _audioManager;
        public PlayerSettings Settings => _settings;
        public PlayerProfile Profile => _profile;
        public LevelProgression LevelProgression => _levelProgression;
        public LevelLoader LevelLoader => _levelLoader;
        public ShopSystem ShopSystem => _shopSystem;
        public UpgradeSystem UpgradeSystem => _upgradeSystem;
        public SpellShop SpellShop => _spellShop;
        public Canvas DragCanvas => _dragCanvas;
        public SpellsConfig SpellsConfig;

        private CurrentLevelSwitcher _levelSwitcher;

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
            _screenSwitcher = new ScreenSwitcher(_events);
            _settings = new PlayerSettings(_storage);
            _upgradeSystem = new UpgradeSystem(_upgrades.Upgrades);
            _shopSystem = new ShopSystem(_profile, _spellShop, _upgradeSystem);
            _levelSelect.Init(_levelProgression, _events);
            _settingsModal.Init(_settings, _audioManager);
            _startScreen.Init(_events, _screenSwitcher, _settingsModal, _offersManager, _profile, _levelProgression);
            _levelLoader = new LevelLoader(_events);
            _levelSwitcher = new CurrentLevelSwitcher(_events, _levelProgression, _profile);
            _events.OnLevelStart.Subscribe(StartLevel);
            Instance = this;
        }

        private async void StartLevel(int levelIndex) => await _levelLoader.LoadLevelWithSceneIndex(levelIndex);
    }
}