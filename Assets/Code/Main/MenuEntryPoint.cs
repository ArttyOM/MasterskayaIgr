using Code.HUD;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Main
{
    public class MenuEntryPoint : MonoBehaviour
    {
        private ServiceLocator _serviceLocator;

        private void Start()
        {
            ProcessStart().Forget();
        }

        private async UniTaskVoid ProcessStart()
        {
            do
            {
                _serviceLocator = ServiceLocator.Instance;
                await UniTask.Yield();
            } while (_serviceLocator == null);
            RestoreSettings();
            if (_serviceLocator.Profile.IsFirstLaunch()) LaunchTutorial();
            else
            {
                _serviceLocator.ScreenSwitcher.ShowScreen(ScreenType.Menu);
            }
            _serviceLocator.Profile.IncrementLaunchCount();
        }

        private void RestoreSettings()
        {
            _serviceLocator.AudioManager.SetMusicVolumeFromNormalized(_serviceLocator.Settings.GetMusicVolume());
            _serviceLocator.AudioManager.SetSoundVolumeFromNormalized(_serviceLocator.Settings.GetSoundVolume());
        }

        private void LaunchTutorial()
        {
            var defaultLevel = _serviceLocator.LevelProgression.DefaultLevel;
            _serviceLocator.LevelLoader.LoadLevelWithSceneIndex(defaultLevel.BuildIndex).Forget();
        }
    }
}