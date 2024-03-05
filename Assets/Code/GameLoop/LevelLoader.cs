using System;
using Code.DebugTools.Logger;
using Code.Events;
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
        public LevelLoader(InGameEvents events)
        {
            
            _toMenuSubscription = events.OnMenu.Subscribe(LoadWithPrepare);
            _restartLevelSubscription = events.OnLevelRestart.Subscribe(LoadAndStart);
        }

        public async void LoadWithPrepare(int x)
        {
            await LoadLevel(x);
            var entryPoint = await WaitForLevelEntry();
            entryPoint.StartPrepare();
        }

        public async void LoadAndStart(int x)
        {
            await LoadLevel(x);
            var entryPoint = await WaitForLevelEntry();
            entryPoint.StartLevel();
        }

        ~LevelLoader()
        {
            _toMenuSubscription.Dispose();
            _restartLevelSubscription.Dispose();
        }


        private async UniTask<LevelEntryPoint> WaitForLevelEntry()
        {
            var entryPoint = Object.FindObjectOfType<LevelEntryPoint>();
            while (!entryPoint.IsLoaded())
            {
                await UniTask.Yield();
            }
            return entryPoint;
        }
        private readonly IDisposable _toMenuSubscription;
        private readonly IDisposable _restartLevelSubscription;
        private async UniTask LoadLevel(int sceneIndex)
        {
            await LoadLevelWithSceneIndex(sceneIndex);
        }

        public async UniTask LoadLevelWithSceneIndex(int sceneIndex)
        {
            $"Загружаем сцену {sceneIndex} SINGLE".Colored(Color.red).Log();
            await Resources.UnloadUnusedAssets();
            await SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
            $"Сцена {sceneIndex} загружена".Colored(Color.red).Log();
        }
    }
}