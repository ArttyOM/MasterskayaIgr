using System;
using System.Threading.Tasks;
using Code.Events;
using Code.GameLoop;
using Code.HUD;
using Code.HUD.ScreenActivators;
using Code.Pools;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Code.Main
{
    [DisallowMultipleComponent]
    public class MainEntryPoint : MonoBehaviour
    {
        [field: SerializeField] public PoolCommonParent PoolCommonParent { get; private set; }

        private ScreenSwitcher _screenSwitcher;
        private InGameEvents _events;
        private LevelLoader _levelLoader;

        [SerializeField] private int _sceneIndex = 1;

        private async void Awake()
        {
            _events = new InGameEvents();
            _screenSwitcher = new ScreenSwitcher();
            var startGameButton = FindObjectOfType<StartGameButton>();
            startGameButton.Init(_screenSwitcher);

            var levelSelectionScreenActivator =
                new LevelSelectionScreenActivator(_events.OnLevelSelection, _screenSwitcher);
            var levelSelectionStarter = FindObjectOfType<LevelSelectionStarter>();
            levelSelectionStarter.Init(_events.OnLevelSelection);

            _levelLoader = new LevelLoader(_screenSwitcher, _events, _sceneIndex);
            await _levelLoader.LoadLevelWithSceneIndex(_sceneIndex);
        }
    }
}