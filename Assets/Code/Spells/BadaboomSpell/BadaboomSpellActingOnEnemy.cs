using System;
using System.Collections.Generic;
using System.Linq;
using Code.Enemies;
using UniRx;
using UnityEngine;

namespace Code.Spells
{
    public class BadaboomSpellActingOnEnemy: IDisposable, ISpellActingOnEnemy
    {
        public void Dispose()
        {
            
        }

        public void Act(SpellExplosion explosion, SpellBalanceConfig spellConfig)
        {
            List<CommonEnemy> enemies = GameObject.FindObjectsByType<CommonEnemy>(FindObjectsSortMode.None).ToList();
            foreach (CommonEnemy enemy in enemies)
            {
                enemy.GetObservableTrigger2DTrigger.OnTriggerEnter2DAsObservable().First()
                    .Subscribe(onNext: collider2D =>
                    {
                        enemy.GetHit(spellConfig.damage);
                    });
            }
            var ps = explosion.GetComponentInChildren<ParticleSystem>();
            Observable.EveryUpdate().SkipWhile(x => ps.IsAlive(true)).First()
                .Subscribe(_ => GameObject.Destroy(explosion.gameObject));
        }
    }
}