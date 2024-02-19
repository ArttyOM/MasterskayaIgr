using Code.Events;
using Code.HUD.Offers;
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


        public void Init(InGameEvents events, ScreenSwitcher screenSwitcher, SettingsModal settingsModal, OffersManager offersManager)
        {
            _events = events;
            _screenSwitcher = screenSwitcher;
            _settings = settingsModal;
            _startButton.onClick.AddListener(OpenLevelSelection);
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

        private void OnDestroy()
        {
            _startButton.onClick.RemoveAllListeners();
            _settingsButton.onClick.RemoveAllListeners();
        }
    }
}