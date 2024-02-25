using System;
using Code.DebugTools.Logger;
using Code.Events;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            
            _toMenuSubscription = events.OnMenu.Subscribe(x =>
            {
                LoadLevel(x).Forget();
            });
            _restartLevelSubscription = events.OnLevelRestart.Subscribe(x => LoadLevel(x).Forget());
        }

        ~LevelLoader()
        {
            _toMenuSubscription.Dispose();
            _restartLevelSubscription.Dispose();
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