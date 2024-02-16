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
            Debug.Log("RestoreSettings!");
            RestoreSettings();
            if (_services.Profile.IsFirstLaunch()) LaunchTutorial();
            else
            {
                Debug.Log($"ScreenSwitcher is {_services.ScreenSwitcher != null}");
                Debug.Log($"services is {_services != null}");
                _services.ScreenSwitcher.ShowScreen(ScreenType.Menu);
                Debug.Log("SHOW MENU SCREEN!");
            }
            _services.Profile.IncrementLaunchCount();
            Debug.Log("LAUNCH COUNT!");
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