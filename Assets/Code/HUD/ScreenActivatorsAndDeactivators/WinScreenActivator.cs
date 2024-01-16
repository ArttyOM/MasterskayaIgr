using System;
using Code.GameLoop;
using UniRx;

namespace Code.HUD.ScreenActivators
{
    public class WinScreenActivator : IDisposable
    {
        private readonly IDisposable _onWinSubscription;
        private readonly ScreenSwitcher _screenSwitcher;

        public WinScreenActivator(ScreenSwitcher screenSwitcher, IObservable<LevelEndResult> onLevelEnd)
        {
            _screenSwitcher = screenSwitcher;
            _onWinSubscription = onLevelEnd.Where(x => x == LevelEndResult.Win).First()
                .Subscribe(_ => ShowLoseScreen());
        }

        public void Dispose()
        {
            _onWinSubscription.Dispose();
        }

        private void ShowLoseScreen()
        {
            _screenSwitcher.ShowScreen(ScreenType.Victory);
        }
    }
}