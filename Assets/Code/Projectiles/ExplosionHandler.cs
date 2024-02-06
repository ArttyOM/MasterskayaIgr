using System;
using System.Collections.Generic;
using System.Linq;
using Code.DebugTools.Logger;
using Code.Spells;
using MyBox;
using UniRx;

namespace Code.Projectiles
{
    public class ExplosionHandler: IDisposable
    {
        public ExplosionHandler(IObservable<ExplosionData> onExplosion, SpellsConfig spellsConfig)
        {
            _spellsConfig = spellsConfig;
            CreatePools();
            _onExplosionSubscription = onExplosion.Subscribe(ShowExplosionAnimationAndEffortOnEnemies);
        }

        private readonly Dictionary<SpellType, SpellExplosionPool> _commonSpellExplosionPool = new();
        private readonly Dictionary<SpellType, SpellExplosionPool> _megaSpellExplosionPool = new();
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
            ShowExplosionAnimation(isMega, explosionData);
            if (isMega)
            {
                "Mega explosion".Colored(Colors.aqua).Log();
            }
            else
            {
                "common explosion".Colored(Colors.aqua).Log();
            }
        }

        private void ShowExplosionAnimation(bool isMega, ExplosionData explosionData)
        {
            if (isMega)
            {
                _megaSpellExplosionPool[explosionData.GetSpellType].Rent(explosionData.GetWorldPosition);
            }
            else
            {
                _commonSpellExplosionPool[explosionData.GetSpellType].Rent(explosionData.GetWorldPosition);
            }
        }
    }
}