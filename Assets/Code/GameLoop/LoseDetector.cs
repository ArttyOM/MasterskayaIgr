using System;
using UniRx;
using UnityEngine;

namespace Code.GameLoop
{
    public class LoseDetector : IDisposable
    {
        public LoseDetector(IObserver<LevelEndResult> eventsLevelEnd)
        {
            var wallCollider = GameObject.FindObjectOfType<WallHealth>()
                .GetComponent<Collider2D>();
            _subscription = Observable.EveryUpdate().SkipWhile(_=>wallCollider.enabled).First()
                .Subscribe(_=> eventsLevelEnd.OnNext(LevelEndResult.Lose));
                
        }
        
        private IDisposable _subscription;
        
        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}