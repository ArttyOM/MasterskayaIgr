using System;
using System.Collections.Generic;
using System.Linq;
using Code.DebugTools.Logger;
using Code.Enemies;
using MyBox;
using UniRx;
using UniRx.Diagnostics;
using UnityEngine;

namespace Code.Spells
{
    public class BadaboomSpellActingOnEnemy: IDisposable, ISpellActingOnEnemy
    {
        public void Dispose()
        {
            
        }

        public void Act(CommonEnemy enemy)
        {
            
        }
        
        public void Act(SpellExplosion explosion, SpellBalanceConfig spellConfig)
        {
            "Act>>>".Colored(Colors.red).Log();
            List<CommonEnemy> enemies = GameObject.FindObjectsByType<CommonEnemy>(FindObjectsSortMode.None).ToList();
            foreach (CommonEnemy enemy in enemies)
             {
                 //enemy.SSSS();
            //     enemy.GetObservableTrigger2DTrigger.OnTriggerEnter2DAsObservable().First()
            //         .Debug()
            //         .Subscribe(onNext: collider2D =>
            //         {
            //             enemy.GetHit(spellConfig.damage);
            //         });
             }
            // enemies[0].GetObservableTrigger2DTrigger.OnTriggerEnter2DAsObservable().First()
            //     .Debug()
            //     .Subscribe(onNext: collider2D =>
            //     {
            //         enemies[0].GetHit(spellConfig.damage);
            //     });
            var ps = explosion.GetComponentInChildren<ParticleSystem>();
            Observable.EveryUpdate().SkipWhile(x => ps.IsAlive(true)).First()
                .Subscribe(_ => GameObject.Destroy(explosion.gameObject));
        }
    }
}