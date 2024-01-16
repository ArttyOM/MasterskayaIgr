using System;
using Code.GameLoop;
using UniRx;

namespace Code.HUD.ScreenActivators
{
    public class LoseScreenActivator : IDisposable
    {
        private readonly IDisposable _onLoseSubscription;
        private readonly ScreenSwitcher _screenSwitcher;

        public LoseScreenActivator(ScreenSwitcher screenSwitcher, IObservable<LevelEndResult> onLevelEnd)
        {
            _screenSwitcher = screenSwitcher;
            _onLoseSubscription = onLevelEnd.Where(x => x == LevelEndResult.Lose).First()
                .Subscribe(_ => ShowLoseScreen());
        }

        public void Dispose()
        {
            _onLoseSubscription.Dispose();
        }

        private void ShowLoseScreen()
        {
            _screenSwitcher.ShowScreen(ScreenType.Defeat);
        }
    }
}