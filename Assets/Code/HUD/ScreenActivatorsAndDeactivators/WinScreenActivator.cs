using System;
using Code.DebugTools.Logger;
using Code.GameLoop;
using MyBox;
using UniRx;
using UnityEngine;

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
                .Subscribe(_ => ShowWinScreen());
        }

        public void Dispose()
        {
            _onWinSubscription.Dispose();
        }

        private void ShowWinScreen()
        {
            $"Win Detected".Colored(Color.red).Log();
            _screenSwitcher.ShowScreen(ScreenType.Victory);
        }
    }
}