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
        
        private IDisposable _onEnemyExploadedSubscription;
        private IObservable<(CommonEnemy, SpellExplosion)> _onEnemyExploded;
        private SpellBalanceConfig _megaSpellConfig;
        private SpellBalanceConfig _commonSpellConfig;

        public void Init(IObservable<(CommonEnemy, SpellExplosion)> onEnemyExploded,
            SpellBalanceConfig commonSpellBalance, SpellBalanceConfig megaSpellConfig)
        {
            _megaSpellConfig = megaSpellConfig;
            _commonSpellConfig = commonSpellBalance;
            _onEnemyExploded = onEnemyExploded;
            _onEnemyExploadedSubscription = _onEnemyExploded.Subscribe(OnExplosion);
        }
        
        public void Dispose()
        {
            _onEnemyExploadedSubscription?.Dispose();
        }

        public void Act(SpellExplosion explosion, SpellBalanceConfig spellConfig)
        {
            
        }

        private void OnExplosion((CommonEnemy enemy, SpellExplosion explosion) enemySpellPair)
        {
            var explosion = enemySpellPair.explosion;
            var enemy = enemySpellPair.enemy;
            

            float damage;
            if (explosion.isMega)
            {
                damage = _megaSpellConfig.damage;
            }
            else
            {
                damage = _commonSpellConfig.damage;
            }
            enemy.GetHit(damage);
        }
    }
}