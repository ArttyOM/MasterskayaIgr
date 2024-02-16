using System.Linq;
using Code.DebugTools.Logger;
using Code.Enemies;
using Code.Events;
using Code.GameLoop;
using Code.HUD;
using Code.HUD.ScreenActivators;
using Code.Projectiles;
using Code.Spells;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine.Rendering.Universal;


namespace Code.Main
{
    public class LevelEntryPoint : MonoBehaviour
    {
        [SerializeField] private WeaponSpawnChanceConfig _weaponSpawnChanceConfig;
        [SerializeField] private SpellsConfig _spellsConfig;
        [SerializeField] private CommonEnemy _commonEnemyPrefab;
        [SerializeField] private float _moveSpeed = 1f;

        private WeaponRandomGenerator _weaponRandomGenerator;
        private SpellVfxGenerator _spellVfxGenerator;
        private GridPointSelector _gridPointSelector;
        private ProjectileThrower _projectileThrower;
        private ExplosionHandler _explosionHandler;

        private ScreenSwitcher _screenSwitcher;
        private InGameEvents _events;
        private LevelLoader _levelLoader;

        private Light2D _globalLight;

        private LoseScreenActivator _loseScreenActivator;
        private WinScreenActivator _winScreenActivator;
        private SpellsPanelActivator _spellsPanelActivator;

        private CommonEnemyMover _commonEnemyMover;


        private async void Start()
        {
            ServiceLocator serviceLocator;
            do
            {
                serviceLocator = ServiceLocator.Instance;
                await UniTask.Yield();
            } while (serviceLocator == null);
            Init(serviceLocator.Events, serviceLocator.ScreenSwitcher);
        }

        public void Init(InGameEvents events, ScreenSwitcher screenSwitcher)
        {
            _events = events;
            ">>LevelEntryPoint.Init".Colored(Color.red).Log();
            
            _weaponRandomGenerator = new WeaponRandomGenerator(_weaponSpawnChanceConfig, _events.OnSpellSelected, _events.OnSessionStart);
            _spellVfxGenerator = new SpellVfxGenerator(_spellsConfig, _events.OnSpellSelected, _events.OnSessionStart);
            _gridPointSelector = new(_events.OnProjectileDestinationSelected);
            
            var weaponPools = _weaponRandomGenerator.GetWeaponPools;
            var spellPools = _spellVfxGenerator.GetSpellPools;
            _projectileThrower = new(_events.OnProjectileDestinationSelected, _events.OnProjectileExploded, weaponPools, spellPools);
            _explosionHandler = new(_events.OnProjectileExploded, _spellsConfig);

            _events = events;
            _screenSwitcher = screenSwitcher;
            _screenSwitcher.HideAllScreensInstantly();
            _screenSwitcher.ShowScreen(ScreenType.PreparationForTheGame);
            _commonEnemyMover = new CommonEnemyMover(_commonEnemyPrefab, _moveSpeed, _events.OnSessionStart);
            InitButtons();
            InitScreenActivators();
        }

        private void OnDestroy()
        {
            ">>LevelEntryPoint OnDestroy".Log();
            _loseScreenActivator?.Dispose();
            _winScreenActivator?.Dispose();
            _spellsPanelActivator?.Dispose();
            
            _weaponRandomGenerator?.Dispose();
            
            _gridPointSelector?.Dispose();
            _spellVfxGenerator?.Dispose();
            _projectileThrower?.Dispose();
            _explosionHandler?.Dispose();
            
            _commonEnemyMover?.Dispose();
        }

        private void InitButtons()
        {
            var startSessionButton = FindObjectOfType<StartSessionButton>();
            startSessionButton.Init(_events.OnSessionStart);
            var spellButtons = Object.FindObjectsOfType<UISelectSpellButton>(true)
                .AsEnumerable();
            if (spellButtons != null)
                spellButtons.ToUniTaskAsyncEnumerable()
                    .ForEachAsync(x => x.Init(_events.OnSpellSelected));
        }


        private void InitScreenActivators()
        {
            _loseScreenActivator?.Dispose();
            _loseScreenActivator = new LoseScreenActivator(_screenSwitcher, _events.OnLevelEnd);
            _winScreenActivator?.Dispose();
            _winScreenActivator = new WinScreenActivator(_screenSwitcher, _events.OnLevelEnd);
            
            _spellsPanelActivator?.Dispose();
            _spellsPanelActivator = new SpellsPanelActivator(_events.OnSessionStart, _events.OnSpellSelected, _events.OnProjectileExploded);

        }
    }
}