using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Code.Enemies;
using Code.Upgrades;
using UniRx;
using UnityEngine;

namespace Code.Spells.IceSpell
{
    public class IceSpellActingOnEnemy: IDisposable, ISpellActingOnEnemy
    {
        private IDisposable _onEnemyExploadedSubscription;
        private IObservable<(CommonEnemy, SpellExplosion)> _onEnemyExploded;
        private SpellBalanceConfig _megaSpellConfig;
        private SpellBalanceConfig _commonSpellConfig;
        private UpgradeService _upgradeService;

        public void Dispose()
        {
            _onEnemyExploadedSubscription?.Dispose();
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
                .Where(x => x.Item2.spellType == SpellType.Ice)
                .Subscribe(OnExplosion);
        }
        
        private void OnExplosion((CommonEnemy enemy, SpellExplosion explosion) enemySpellPair)
        {
            var explosion = enemySpellPair.explosion;
            var enemy = enemySpellPair.enemy;
            

            float damage;
            float slowValue;
            float slowDuration;
            if (explosion.isMega)
            {
                damage = _megaSpellConfig.damage;
                slowValue= _megaSpellConfig.damagePerSecond;
                slowDuration = _megaSpellConfig.duration;
            }
            else
            {
                damage = _commonSpellConfig.damage;
                slowValue = _commonSpellConfig.slowValue;
                slowDuration = _commonSpellConfig.duration;
            }
            enemy.GetHit(_upgradeService.GetUpgradedValue(UpgradeTarget.SpellDamage, damage));
            
            MainThreadDispatcher
                .StartUpdateMicroCoroutine(SlowDebuffMicrocoroutine(enemy ,slowValue, slowDuration));
        }

        private IEnumerator SlowDebuffMicrocoroutine(CommonEnemy enemy,float slowValue, float slowDuration)
        {
            enemy.currentSpeed *= (1 - slowValue);
            
            while (slowDuration >= 0)
            {
                slowDuration -= Time.deltaTime;
                yield return null;
            }
            if (enemy != null)
            {
                enemy.currentSpeed = enemy.GetBaseSpeed;
            }
        }
    }
}