using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Code.Enemies;
using Code.Upgrades;
using UniRx;
using UnityEngine;

namespace Code.Spells.PoisonSpell
{
    public class PoisonSpellActingOnEnemy: IDisposable, ISpellActingOnEnemy
    {
        private IDisposable _onEnemyExploadedSubscription;
        private IObservable<(CommonEnemy, SpellExplosion)> _onEnemyExploded;
        private SpellBalanceConfig _megaSpellConfig;
        private SpellBalanceConfig _commonSpellConfig;
        private UpgradeService _upgradeService;

        public void Dispose()
        {
        }

        public void Act(SpellExplosion explosion, SpellBalanceConfig spellConfig)
        {
        }

        public void Init(IObservable<(CommonEnemy, SpellExplosion)> onEnemyExploded,
            SpellBalanceConfig commonSpellBalance, SpellBalanceConfig megaSpellConfig, UpgradeService upgradeService)
        {
            _upgradeService = upgradeService;
            _megaSpellConfig = megaSpellConfig;
            _commonSpellConfig = commonSpellBalance;
            _onEnemyExploded = onEnemyExploded;
            _onEnemyExploadedSubscription = _onEnemyExploded
                .Where(x => x.Item2.spellType == SpellType.Poison)
                .Subscribe(OnExplosion);
        }
        
        private void OnExplosion((CommonEnemy enemy, SpellExplosion explosion) enemySpellPair)
        {
            var explosion = enemySpellPair.explosion;
            var enemy = enemySpellPair.enemy;
            
            float damage;
            float damagePerSecond;
            float duration;
            if (explosion.isMega)
            {
                damage = _megaSpellConfig.damage;
                damagePerSecond= _megaSpellConfig.damagePerSecond;
                duration = _megaSpellConfig.duration;
            }
            else
            {
                damage = _commonSpellConfig.damage;
                damagePerSecond = _commonSpellConfig.damagePerSecond;
                duration = _commonSpellConfig.duration;
            }
            enemy.GetHit(_upgradeService.GetUpgradedValue(UpgradeTarget.SpellDamage, damage));
            
            MainThreadDispatcher
                .StartUpdateMicroCoroutine(PoisonDebuffMicrocoroutine(enemy ,damagePerSecond, duration));
        }
        
        private IEnumerator PoisonDebuffMicrocoroutine(CommonEnemy enemy,float damagePerSecond, float duration)
        {
            float deltaTime = 0;
            while (duration >= 0 && (enemy is not null))
            {
                deltaTime += Time.deltaTime;
                duration -= Time.deltaTime;
                if (enemy == null) yield break;
                if (deltaTime >= 1f)
                {
                    enemy.GetHit(damagePerSecond * deltaTime);
                    deltaTime = 0;
                }
                yield return null;
            }

            if (deltaTime > 0 && enemy != null)
            {
                enemy.GetHit(damagePerSecond * deltaTime);
            }
        }
    }
}