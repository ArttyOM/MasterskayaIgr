using System;
using Code.DebugTools.Logger;
using Code.Events;
using Code.HUD;
using Code.HUD.ScreenActivators;
using Code.Main;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Code.GameLoop
{
    /// <summary>
    /// Грузит и перезагружает уровни.
    /// Инициализирует зависимости после загрузки
    /// </summary>
    public class LevelLoader
    {
        public LevelLoader(ScreenSwitcher screenSwitcher, InGameEvents events, int sceneIndex)
        {
            _events = events;

            _sceneIndex = sceneIndex;
            _screenSwitcher = screenSwitcher;

            _toMenuSubscription = _events.OnMenu.Subscribe(x =>
            {
                LoadLevel(x).ToObservable()
                    .Subscribe(_ =>
                    {
                        ">>ShowScreen Menu".Colored(Color.red).Log();
                        _screenSwitcher.ShowScreen(ScreenType.Menu);
                    });
            });
            _restartLevelSubscription = _events.OnLevelRestart.Subscribe(x => LoadLevel(x));
        }

        ~LevelLoader()
        {
            _toMenuSubscription.Dispose();
            _restartLevelSubscription.Dispose();
        }


        private InGameEvents _events;
        private readonly ScreenSwitcher _screenSwitcher;

        private IDisposable _toMenuSubscription;
        private IDisposable _restartLevelSubscription;

        private int _sceneIndex;

        private async UniTask LoadLevel(int sceneIndex)
        {
            await LoadLevelWithSceneIndex(sceneIndex);
        }

        public async UniTask LoadLevelWithSceneIndex(int sceneIndex)
        {
            _sceneIndex = sceneIndex;

            var scene = SceneManager.GetSceneByBuildIndex(sceneIndex);

            if (SceneManager.GetActiveScene().buildIndex != _sceneIndex)
            {
                $"Загружаем сцену {_sceneIndex} ADDITIVE".Colored(Color.red).Log();
                await SceneManager.LoadSceneAsync(_sceneIndex, LoadSceneMode.Additive);
            }
            else
            {
                $"Загружаем сцену {_sceneIndex} SINGLE".Colored(Color.red).Log();
                await SceneManager.LoadSceneAsync(_sceneIndex);
            }

            $"Сцена {_sceneIndex} загружена".Colored(Color.red).Log();

            if (scene.isLoaded)
            {
                $"Выгружаем сцену {scene.buildIndex}".Colored(Color.red).Log();
                await SceneManager.UnloadSceneAsync(scene);
            }

            var levelEntryPoint = Object.FindObjectOfType<LevelEntryPoint>();
            if (levelEntryPoint != null) await levelEntryPoint.Init(_events, _screenSwitcher, _sceneIndex);
        }
    }
}