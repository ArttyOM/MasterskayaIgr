using Code.Audio;
using Code.DebugTools.Logger;
using Code.Events;
using Code.GameLoop;
using Code.HUD;
using Code.HUD.Gameplay;
using Code.HUD.Offers;
using Code.HUD.Start;
using Code.InGameRewards;
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
        [SerializeField] private StartScreen _startScreen;
        [SerializeField] private SettingsModal _settingsModal;
        [SerializeField] private OffersManager _offersManager;
        [SerializeField] private SpellShop _spellShop;
        [SerializeField] private UpgradeList _upgrades;
        [SerializeField] private AudioManager _audioManager;
        [SerializeField] private Canvas _dragCanvas;
        [SerializeField] private DropRewards _dropRewards;
        [SerializeField] private Camera _camera;
        [SerializeField] private GameplayScreen _gameplayScreen;
        [SerializeField] private PrepareScreen _prepareScreen;
        
        
        
        [SerializeField] private DefaultPlayerProfile _defaultProfile;
        
        
        
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
        public DropRewards DropRewardsService => _dropRewards;
        public Camera Camera => _camera;

        public PrepareScreen PrepareScreen => _prepareScreen;
        public GameplayScreen GameplayScreen => _gameplayScreen;

        public SpellsConfig SpellsConfig;

        private LevelCompleteHandler _levelCompleteHandler;
        

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
            _profile = new PlayerProfile(_storage, _defaultProfile);
            _screenSwitcher = new ScreenSwitcher(_events);
            _settings = new PlayerSettings(_storage);
            _upgradeSystem = new UpgradeSystem(_upgrades.Upgrades);
            _shopSystem = new ShopSystem(_profile, _spellShop, _upgradeSystem);
            _settingsModal.Init(_settings, _audioManager);
            _startScreen.Init(_events, _screenSwitcher, _settingsModal, _profile, _levelProgression);
            
            _levelLoader = new LevelLoader(_events);
            _levelCompleteHandler = new LevelCompleteHandler(_events, _levelProgression, _profile, _dropRewards);
            _events.OnLevelStart.Subscribe(StartLevel);
            Instance = this;
        }

        private void StartLevel(int levelIndex)
        {
            _levelLoader.LoadWithPrepare(levelIndex);
        }
    }
}