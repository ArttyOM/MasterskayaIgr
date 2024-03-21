using System;
using Code.DebugTools.Logger;
using Code.Enemies;
using Code.Upgrades;
using UniRx;


namespace Code.Spells.NoSpell
{
    public class NoSpellActingOnEnemy : IDisposable, ISpellActingOnEnemy
    {
        private IDisposable _onEnemyExploadedSubscription;
        private IObservable<(CommonEnemy, SpellExplosion)> _onEnemyExploded;
        private SpellBalanceConfig _megaSpellConfig;
        private SpellBalanceConfig _commonSpellConfig;
        private UpgradeService _upgradeService;

        public void Init(IObservable<(CommonEnemy, SpellExplosion)> onEnemyExploded, 
            SpellBalanceConfig commonSpellBalance, SpellBalanceConfig megaSpellConfig,
            UpgradeService upgradeService)
        {
            $">>Init Обычный взрыв".Log();

            _upgradeService = upgradeService;
            _megaSpellConfig = megaSpellConfig;
            _commonSpellConfig = commonSpellBalance;
            _onEnemyExploded = onEnemyExploded;
            _onEnemyExploadedSubscription = _onEnemyExploded
                .Where(x => x.Item2.spellType == SpellType.NoSpell)
                .Subscribe(OnExplosion);
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

            $">>OnExplosion Обычный взрыв с уроном {damage}".Log();
            enemy.GetHit(_upgradeService.GetUpgradedValue(UpgradeTarget.AutofireDamage, damage));
        }
    }
}
