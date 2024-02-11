using System;
using System.Collections.Generic;
using System.Linq;
using Code.Enemies;
using UniRx;
using UnityEngine;

namespace Code.Spells.IceSpell
{
    public class IceSpellActingOnEnemy: IDisposable, ISpellActingOnEnemy
    {
        public void Dispose()
        {
        }

        public void Act(SpellExplosion explosion, SpellBalanceConfig spellConfig)
        {
            List<CommonEnemy> enemies = GameObject.FindObjectsByType<CommonEnemy>(FindObjectsSortMode.None).ToList();
            foreach (CommonEnemy enemy in enemies)
            {
                enemy.GetObservableTrigger2DTrigger.OnTriggerEnter2DAsObservable()
                    .Subscribe(onNext: collider2D =>
                    {
                        enemy.GetHit(spellConfig.damage);
                    });
            }
            var provider = explosion.GetColliderProvider;
            var collider = provider.GetCollider2D;
        }
    }
}