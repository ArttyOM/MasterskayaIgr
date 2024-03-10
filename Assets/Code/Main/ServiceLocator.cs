using System;
using Code.Audio;
using Code.DebugTools.Logger;
using Code.Events;
using Code.GameLoop;
using Code.HUD;
using Code.HUD.DamageNumbers;
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
        [SerializeField] private GuideModal _guideModal;
        [SerializeField] private OffersManager _offersManager;
        [SerializeField] private SpellDefinitions _spellShop;
        [SerializeField] private UpgradeList _upgrades;
        [SerializeField] private AudioManager _audioManager;
        [SerializeField] private Canvas _dragCanvas;
        [SerializeField] private DropRewards _dropRewards;
        [SerializeField] private Camera _camera;
        [SerializeField] private GameplayScreen _gameplayScreen;
        [SerializeField] private PrepareScreen _prepareScreen;
        [SerializeField] private DamageNumbersManager _damageNumbers;
        
        
        
        
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
        public SpellDefinitions SpellShop => _spellShop;
        public Canvas DragCanvas => _dragCanvas;
        public DropRewards DropRewardsService => _dropRewards;
        public Camera Camera => _camera;
        public DamageNumbersManager DamageNumbers => _damageNumbers;
        public GuideModal GuideModal => _guideModal;
        public ModalsManager Modals => _modalsManager;

        public PrepareScreen PrepareScreen => _prepareScreen;
        public GameplayScreen GameplayScreen => _gameplayScreen;

        public SpellsConfig SpellsConfig;

        private LevelCompleteHandler _levelCompleteHandler;
        private ModalsManager _modalsManager;
        private IDisposable _settingsSubscription;
        private IDisposable _guideSubscribtion;

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
            _modalsManager = new ModalsManager();
            _storage = new PlayerPrefsStorage();
            _profile = new PlayerProfile(_storage, _defaultProfile);
            _screenSwitcher = new ScreenSwitcher(_events);
            _settings = new PlayerSettings(_storage);
            _upgradeSystem = new UpgradeSystem(_upgrades.Upgrades);
            _shopSystem = new ShopSystem(_profile, _spellShop, _upgradeSystem);
            _settingsModal.Init(_settings, _audioManager);
            _settingsModal.GetComponent<ModalScreen>().Init(_modalsManager);
            _guideModal.GetComponent<ModalScreen>().Init(_modalsManager);
            _settingsSubscription = _events.OnSettingsRequested.Subscribe(OpenSettings);
            _guideSubscribtion = _events.OnGuideRequested.Subscribe(OpenGuide);
            _modalsManager.Deactivate();
            _startScreen.Init(_events, _screenSwitcher, _profile, _levelProgression);
            
            _levelLoader = new LevelLoader(_events);
            _levelCompleteHandler = new LevelCompleteHandler(_events, _levelProgression, _profile, _dropRewards);
            _events.OnLevelStart.Subscribe(StartLevel);
            Instance = this;
        }

        private void OpenGuide(Unit unit)
        {
            _modalsManager.Activate(_guideModal.gameObject.name);
        }
        private void OpenSettings(Unit unit)
        {
            _modalsManager.Activate(_settingsModal.gameObject.name);
        }

        private void StartLevel(int levelIndex)
        {
            _levelLoader.LoadWithPrepare(levelIndex);
        }

        private void OnDestroy()
        {
            _settingsSubscription.Dispose();
            _guideSubscribtion.Dispose();
        }

        public event Action<float, float> OnWallHpChanged;

        public void ChangeWallHp(float healthPoints, float maxHealthPoints)
        {
            OnWallHpChanged?.Invoke(healthPoints, maxHealthPoints);
        }
    }
}