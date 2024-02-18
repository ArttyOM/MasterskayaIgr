using System;
using Code.DebugTools.Logger;
using MyBox;
using UniRx;

namespace Code.GameLoop
{
    public class WinDetector: IDisposable
    {
        public WinDetector(Enemies.Enemies enemies, IObserver<LevelEndResult> eventsLevelEnd)
        {
            // Observable.Interval(TimeSpan.FromMilliseconds(1000))
            //     .Subscribe(_ => $"alive enemies: {enemies.GetAliveEnemies.Count}".Colored(Colors.red).Log());
            _subscription = Observable.EveryUpdate().Where(_ =>enemies.GetAliveEnemies.Count == 0).First()
                .Subscribe(_=> eventsLevelEnd.OnNext(LevelEndResult.Win));
        }

        private IDisposable _subscription;
        
        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}