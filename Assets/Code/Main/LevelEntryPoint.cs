using Code.DebugTools.Logger;
using Code.Enemies;
using Code.Events;
using Code.GameLoop;
using Code.HUD;
using Code.HUD.ScreenActivators;
using Code.InGameRewards;
using Code.Projectiles;
using Code.Saves;
using Code.Spells;
using Code.Upgrades;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;


namespace Code.Main
{
    public class LevelEntryPoint : MonoBehaviour
    {
        [SerializeField] private WeaponSpawnChanceConfig _weaponSpawnChanceConfig;
        [SerializeField] private SpellsConfig _spellsConfig;
        [SerializeField] private EnemiesConfig _enemiesConfig;
        [SerializeField] private AutofireConfig _autofireConfig;

        private OnClickProjectileThrower _onClickProjectileThrower;
        
        private Enemies.Enemies _enemies;
        
        private WinDetector _winDetector;
        private LoseDetector _loseDetector;

        private ScreenSwitcher _screenSwitcher;
        private InGameEvents _events;
        private LevelLoader _levelLoader;

        private Light2D _globalLight;

        private LoseScreenActivator _loseScreenActivator;
        private WinScreenActivator _winScreenActivator;

        private CommonEnemyMover _commonEnemyMover;
        private EnemyDropService _dropService;
        private bool _isLoaded = false;


        private async void Start()
        {
            ServiceLocator serviceLocator;
            do
            {
                serviceLocator = ServiceLocator.Instance;
                await UniTask.Yield();
            } while (serviceLocator == null);
            Init(serviceLocator.Events, serviceLocator.ScreenSwitcher, serviceLocator.Profile, serviceLocator.UpgradeSystem, serviceLocator.Camera, serviceLocator.DropRewardsService);
        }

        public void Init(InGameEvents events, ScreenSwitcher screenSwitcher, PlayerProfile profile, UpgradeSystem upgradeSystem, Camera camera, DropRewards dropRewardsService)
        {
            ClearDisposables();
            _events = events;
            ">>LevelEntryPoint.Init".Colored(Color.red).Log();

            _enemies = new(_enemiesConfig, _events.OnEnemyDead);
            ConfigHolder configHolder = new ConfigHolder(_autofireConfig, _weaponSpawnChanceConfig, _spellsConfig);
            _onClickProjectileThrower = new OnClickProjectileThrower(configHolder,_events, profile, upgradeSystem);
            
            _winDetector = new WinDetector(_enemies, _events.OnLevelEnd);
            _loseDetector = new LoseDetector(_events.OnLevelEnd);
            
           _events = events;
            _screenSwitcher = screenSwitcher;
            _screenSwitcher.HideAllScreensInstantly();
            _screenSwitcher.ShowScreen(ScreenType.PreparationForTheGame);
            _commonEnemyMover = new CommonEnemyMover(_enemiesConfig, _enemies, _events.OnSessionStart);
            _dropService = new EnemyDropService(_events.OnEnemyDead, _enemiesConfig, camera, dropRewardsService);
            InitScreenActivators();
            InitEnemies();
            _isLoaded = true;
        }

        private void OnDisable()
        {
            ClearDisposables();
        }

        private void ClearDisposables()
        {
            ">>LevelEntryPoint OnDestroy".Log();
            _dropService?.Dispose();
            _loseDetector?.Dispose();
            _winDetector?.Dispose();
            _loseScreenActivator?.Dispose();
            _winScreenActivator?.Dispose();
            
            _onClickProjectileThrower?.Dispose();
            
            _enemies?.Dispose();
            _commonEnemyMover?.Dispose();
        }

        private void InitEnemies()
        {
            foreach (var enemy in _enemies.GetAliveEnemies)
            {
                enemy.Init(_events.OnExplosionEnter, _events.OnEnemyDead, _enemies.GetEnemyStats[enemy.GetEnemyType]);
            }
        }
        
        private void InitScreenActivators()
        {
            _loseScreenActivator?.Dispose();
            _loseScreenActivator = new LoseScreenActivator(_screenSwitcher, _events.OnLevelEnd);
            _winScreenActivator?.Dispose();
            _winScreenActivator = new WinScreenActivator(_screenSwitcher, _events.OnLevelEnd);
        }

        public void StartPrepare()
        {
            _screenSwitcher.HideAllScreensInstantly();
            _screenSwitcher.ShowScreen(ScreenType.PreparationForTheGame);
        }

        public void StartLevel()
        {
            _events.OnSessionStart.OnNext(1);
        }

        public bool IsLoaded()
        {
            return _isLoaded;
        }
    }
}