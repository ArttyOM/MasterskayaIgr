using System;
using System.Collections.Generic;
using System.Linq;
using Code.DebugTools.Logger;
using Code.Enemies;
using Code.Spells;
using Code.Spells.IceSpell;
using Code.Spells.LineAttackSpell;
using Code.Spells.MinesSpell;
using Code.Spells.PoisonSpell;
using Code.Spells.ShrapnelSpell;
using MyBox;
using UniRx;

namespace Code.Projectiles
{
    public class ExplosionHandler: IDisposable
    {
        public ExplosionHandler(IObservable<ExplosionData> onExplosion, IObservable<(CommonEnemy, SpellExplosion)> onEnemyExploded, SpellsConfig spellsConfig)
        {
            _spellsConfig = spellsConfig;
            CreatePools();
            _onExplosionSubscription = onExplosion.Subscribe(ShowExplosionAnimationAndEffortOnEnemies);
            _interationWithEnemies = new()
            {
                {SpellType.Badaboom, new BadaboomSpellActingOnEnemy()},
                {SpellType.Poison, new PoisonSpellActingOnEnemy()},
                {SpellType.Shrapnel, new ShrapnelSpellActingOnEnemy()},
                {SpellType.LineAttack, new LineAttackSpellActingOnEnemy()},
                {SpellType.Ice, new IceSpellActingOnEnemy()},
                {SpellType.Mine, new MineSpellActingOnEnemy()}
            };

            foreach (var config in spellsConfig.spellConfigs)
            {
                _interationWithEnemies[config.spellType].Init(onEnemyExploded, config.commonSpellBalance, config.megaSpellBalance);
                //_interationWithEnemies[config.spellType] 
            }
            //onEnemyExploded, spellsConfig.spellConfigs.
           
            
        }

        private readonly Dictionary<SpellType, SpellExplosionPool> _commonSpellExplosionPool = new();
        private readonly Dictionary<SpellType, SpellExplosionPool> _megaSpellExplosionPool = new();

        private readonly Dictionary<SpellType, ISpellActingOnEnemy> _interationWithEnemies;
        private readonly SpellsConfig _spellsConfig;
        private readonly IDisposable _onExplosionSubscription;
        
        public void Dispose()
        {
            _onExplosionSubscription?.Dispose();
        }

        private void CreatePools()
        {
            SpellExplosion commonPrefab;
            SpellExplosion megaPrefab;
            SpellType spellType;
            foreach (var spellConfig in  _spellsConfig.spellConfigs)
            {
                commonPrefab = spellConfig.commonSpellBalance.spellExplosionVfxPrefab;
                megaPrefab = spellConfig.megaSpellBalance.spellExplosionVfxPrefab;
                spellType = spellConfig.spellType;
                _commonSpellExplosionPool.Add(spellType, new SpellExplosionPool(commonPrefab));
                _megaSpellExplosionPool.Add(spellType, new SpellExplosionPool(megaPrefab));
            }
        }
        
        private void ShowExplosionAnimationAndEffortOnEnemies(ExplosionData explosionData)
        {
            SpellType spellType = explosionData.GetSpellType;
            SpellConfig spellConfig = _spellsConfig.spellConfigs.Find(x => x.spellType == spellType);

            bool isMega = spellConfig.megaCastWeaponType == explosionData.GetProjectileType;
            var explosion = InstantiateExplosion(isMega, explosionData);
            if (isMega)
            {
                _interationWithEnemies[spellType].Act(explosion, spellConfig.megaSpellBalance);
                "Mega explosion".Colored(Colors.aqua).Log();
            }
            else
            {
                _interationWithEnemies[spellType].Act(explosion, spellConfig.commonSpellBalance);
                "common explosion".Colored(Colors.aqua).Log();
            }
        }

        private SpellExplosion InstantiateExplosion(bool isMega, ExplosionData explosionData)
        {
            if (isMega)
            {
                return _megaSpellExplosionPool[explosionData.GetSpellType].Rent(explosionData.GetWorldPosition, true);
            }
            else
            {
                return _commonSpellExplosionPool[explosionData.GetSpellType].Rent(explosionData.GetWorldPosition, false);
            }
        }
    }
}