using System;
using Code.DebugTools.Logger;
using Code.GameLoop;
using Code.Main;
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
            var services = ServiceLocator.Instance;
            var levelIndex = services.Profile.GetCurrentLevel();
            var level = services.LevelProgression.GetLevel(levelIndex);
            var completed = services.Profile.IsLevelCompleted(levelIndex);
            services.Profile.CompleteLevel(levelIndex);
            var nextLevel = services.LevelProgression.GetNext(level);
            if (nextLevel < 0)
            {
                nextLevel = levelIndex;
            }
            services.Profile.SetCurrentLevel(nextLevel);
            if (!completed)
            {
                services.DropRewardsService.DropCoins(level.CoinsReward, new (Screen.width/2f, Screen.height/2f));
            }
            _screenSwitcher.HideAllScreensInstantly();
            _screenSwitcher.ShowScreen(ScreenType.Victory);
        }
    }
}