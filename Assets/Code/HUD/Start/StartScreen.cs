using Code.Events;
using Code.HUD.Offers;
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
        [SerializeField] private Button _settingsButton;
        [SerializeField] private OffersList _offersList;
        
        
        private SettingsModal _settings;
        private InGameEvents _events;
        private ScreenSwitcher _screenSwitcher;
        private PlayerProfile _profile;
        private LevelProgression _levelProgression;


        public void Init(InGameEvents events, ScreenSwitcher screenSwitcher, 
            SettingsModal settingsModal, OffersManager offersManager, PlayerProfile profile, LevelProgression levelProgression)
        {
            _levelProgression = levelProgression;
            _profile = profile;
            _events = events;
            _screenSwitcher = screenSwitcher;
            _settings = settingsModal;
            #if UNITY_EDITOR && LEVEL_SELECTOR
            _startButton.onClick.AddListener(OpenLevelSelection);
            #else 
            _startButton.onClick.AddListener(OpenNextLevel);
            #endif
            _settingsButton.onClick.AddListener(OpenSettings);
            _offersList.Render(offersManager.GetRandomOffers());
        }

        private void OpenSettings() => _settings.Show();

        private void OpenLevelSelection()
        {
            _events.OnLevelSelection.OnNext(new Unit());
            _screenSwitcher.HideAllScreensInstantly();
            _screenSwitcher.ShowScreen(ScreenType.LevelSelector);
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
            _settingsButton.onClick.RemoveAllListeners();
        }
    }
}