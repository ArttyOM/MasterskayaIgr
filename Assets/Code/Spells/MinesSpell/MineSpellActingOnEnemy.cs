using System;
using System.Collections.Generic;
using System.Linq;
using Code.Enemies;
using Code.Upgrades;
using UniRx;
using UnityEngine;

namespace Code.Spells.MinesSpell
{
    public class MineSpellActingOnEnemy: IDisposable, ISpellActingOnEnemy
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

        }

        public void Init(IObservable<(CommonEnemy, SpellExplosion)> onEnemyExploded,
            SpellBalanceConfig commonSpellBalance, SpellBalanceConfig megaSpellConfig, UpgradeService upgradeService)
        {
        }
    }
}