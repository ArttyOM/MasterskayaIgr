using Code.Events;
using Code.Levels;
using Code.Saves;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Code.HUD.Start
{
    public class StartScreen : MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _guideButton;
        
        
        private InGameEvents _events;
        private ScreenSwitcher _screenSwitcher;
        private PlayerProfile _profile;
        private LevelProgression _levelProgression;


        public void Init(InGameEvents events, ScreenSwitcher screenSwitcher, PlayerProfile profile, LevelProgression levelProgression)
        {
            _levelProgression = levelProgression;
            _profile = profile;
            _events = events;
            _screenSwitcher = screenSwitcher;
            _startButton.onClick.RemoveAllListeners();
            _startButton.onClick.AddListener(OpenNextLevel);
            _quitButton.onClick.AddListener(Application.Quit);
            _settingsButton.onClick.RemoveAllListeners();
            _settingsButton.onClick.AddListener(OpenSettings);
            _guideButton.onClick.RemoveAllListeners();
            _guideButton.onClick.AddListener(OpenGuide);
        }

        private void OpenGuide()
        {
            _events.OnGuideRequested.OnNext(new Unit());
        }

        private void OpenSettings()
        {
            _events.OnSettingsRequested.OnNext(new Unit());
        }

        private void OpenNextLevel()
        {
            var currentLevel = _profile.GetCurrentLevel();
            var level = _levelProgression.GetLevel(currentLevel);
            if (level.BuildIndex == -1)
            {
                Debug.Log($"Requested Level is not found in level progression {currentLevel}");
                return;
            }
            _events.OnLevelStart.OnNext(level.BuildIndex);
        }

        private void OnDestroy()
        {
            _startButton.onClick.RemoveAllListeners();
            _guideButton.onClick.RemoveAllListeners();
            _settingsButton.onClick.RemoveAllListeners();
            _quitButton.onClick.RemoveAllListeners();
        }
    }
}