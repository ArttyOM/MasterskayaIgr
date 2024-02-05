using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
using UniRx;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;


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

        private ScreenSwitcher _screenSwitcher;
        private InGameEvents _events;
        private int _sceneIndex;

        private bool _isInit = false;

        private LevelLoader _levelLoader;

        private Light2D _globalLight;

        private LoseScreenActivator _loseScreenActivator;
        private WinScreenActivator _winScreenActivator;
        private SpellsPanelActivator _spellsPanelActivator;

        private CommonEnemyMover _commonEnemyMover;
        
        public async UniTask Init(InGameEvents events, ScreenSwitcher screenSwitcher, int sceneIndex)
        {
            ">>LevelEntryPoint.Init".Colored(Color.red).Log();
            
            _weaponRandomGenerator = new WeaponRandomGenerator(_weaponSpawnChanceConfig, _events.OnSpellSelected, _events.OnSessionStart);
            _spellVfxGenerator = new SpellVfxGenerator(_spellsConfig, _events.OnSpellSelected, _events.OnSessionStart, _events.OnProjectileDestinationSelected);
            _gridPointSelector = new(_events.OnProjectileDestinationSelected);
            
            var weaponPools = _weaponRandomGenerator.GetWeaponPools;
            var spellPools = _spellVfxGenerator.GetSpellPools;
            _projectileThrower = new(_events.OnProjectileDestinationSelected, weaponPools, spellPools);

            var eventSystem = FindObjectOfType<EventSystem>();
            if (eventSystem is null)
            {
                var uiEvents = new GameObject("UiInputEvents");
                uiEvents.AddComponent<EventSystem>();
                uiEvents.AddComponent<StandaloneInputModule>();
            }

            _sceneIndex = sceneIndex;

            _events = events;
            _screenSwitcher = screenSwitcher;

            _screenSwitcher.ReInit();
            _screenSwitcher.ShowScreen(ScreenType.PreparationForTheGame);

            _commonEnemyMover = new CommonEnemyMover(_commonEnemyPrefab, _moveSpeed, _events.OnSessionStart);

            InitButtons();
            InitScreenActivators();

            foreach (var light2D in FindObjectsOfType<Light2D>(true))
                if (light2D.lightType == Light2D.LightType.Global)
                {
                    _globalLight = light2D;
                    _globalLight.enabled = true;
                    break;
                }

            _isInit = true;
        }

        private void Awake()
        {
            Observable.TimerFrame(3).First().Where(_ => !_isInit).Subscribe(_ =>
            {
                "WARNING! Автоинициализация уровня запущена".Colored(Color.yellow).LogWarning();
                _events = new InGameEvents();
                _screenSwitcher = new ScreenSwitcher();
                _sceneIndex = SceneManager.GetActiveScene().buildIndex;

                Init(_events, _screenSwitcher, _sceneIndex);

                _levelLoader = new LevelLoader(_screenSwitcher, _events, _sceneIndex);
            });
        }

        private void OnDestroy()
        {
            ">>LevelEntryPoint OnDestroy".Log();
            _loseScreenActivator?.Dispose();
            _winScreenActivator?.Dispose();
            _spellsPanelActivator?.Dispose();
            
            _commonEnemyMover?.Dispose();
            _weaponRandomGenerator?.Dispose();
            
            _gridPointSelector?.Dispose();
            _spellVfxGenerator?.Dispose();
            _projectileThrower?.Dispose();
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
            _spellsPanelActivator = new SpellsPanelActivator(_events.OnSessionStart, _events.OnSpellSelected, _events.OnProjectileDestinationSelected);

        }
    }
}