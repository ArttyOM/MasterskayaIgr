using System;
using UniRx;
using UnityEngine;

namespace Code.Spells
{
    public class SpellExplosion:MonoBehaviour
    {
        public SpellType spellType;
        public bool isMega;

        private IDisposable _subscription;
        public void VfxDestroyWithDelay()
        {
            var ps = this.GetComponentInChildren<ParticleSystem>();
            _subscription = Observable.EveryUpdate().SkipWhile(x => ps.IsAlive(true)).First()
                .Subscribe(_ => GameObject.Destroy(this.gameObject));
        }

        private void OnDestroy()
        {
            _subscription?.Dispose();
        }
    }
}