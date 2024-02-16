using Code.HUD;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Main
{
    public class MenuEntryPoint : MonoBehaviour
    {
        private MainEntryPoint _services;

        private void Start()
        {
            ProcessStart().Forget();
        }

        private async UniTaskVoid ProcessStart()
        {
            do
            {
                _services = MainEntryPoint.Instance;
                await UniTask.Yield();
            } while (_services == null);
            RestoreSettings();
            if (_services.Profile.IsFirstLaunch()) LaunchTutorial();
            else
            {
                _services.ScreenSwitcher.ShowScreen(ScreenType.Menu);
            }
            _services.Profile.IncrementLaunchCount();
        }

        private void RestoreSettings()
        {
            _services.AudioManager.SetMusicVolumeFromNormalized(_services.Settings.GetMusicVolume());
            _services.AudioManager.SetSoundVolumeFromNormalized(_services.Settings.GetSoundVolume());
        }

        private void LaunchTutorial()
        {
            var defaultLevel = _services.LevelProgression.DefaultLevel;
            _services.LevelLoader.LoadLevelWithSceneIndex(defaultLevel.BuildIndex).Forget();
        }
    }
}