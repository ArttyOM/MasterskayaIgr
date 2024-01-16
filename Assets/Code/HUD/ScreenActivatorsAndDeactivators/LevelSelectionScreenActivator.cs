using System;
using UniRx;

namespace Code.HUD.ScreenActivators
{
    public class LevelSelectionScreenActivator : IDisposable
    {
        private readonly IDisposable _onLevelSelectionSubscription;
        private readonly ScreenSwitcher _screenSwitcher;

        public LevelSelectionScreenActivator(IObservable<Unit> onLevelSelection, ScreenSwitcher screenSwitcher)
        {
            _screenSwitcher = screenSwitcher;
            _onLevelSelectionSubscription = onLevelSelection.Subscribe(_ => ShowScreen());
        }

        public void ShowScreen()
        {
            _screenSwitcher.ShowScreen(ScreenType.LevelSelector);
        }

        public void Dispose()
        {
            _onLevelSelectionSubscription.Dispose();
        }
    }
}