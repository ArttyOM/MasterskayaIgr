using System;
using System.Collections.Generic;
using Code.Projectiles;
using GameAnalyticsSDK.Setup;
using UniRx;
using UnityEngine;

namespace Code.Spells
{
    public class SpellVfxGenerator: IDisposable
    {
        public SpellVfxGenerator(SpellsConfig spellsConfig, IObservable<SpellType> onSpellSelected)
        {
            CreatePools(spellsConfig);
            
            _onSpellSelectedSubsctiption = onSpellSelected.Subscribe(ApplyVfxToCurrentItem);
        }

        private IDisposable _onSpellSelectedSubsctiption;

        private readonly Dictionary<SpellType, SpellPool> _spellPools = new();

        private Weapon _currentWeapon;
        private Weapon _nextWeapon;

        private void CreatePools(SpellsConfig spellsConfig)
        {
            Spell prefab;
            SpellType spellType;
            foreach (var spellConfig in  spellsConfig.spellConfigs)
            {
                prefab = spellConfig.spellVfxPrefab;
                spellType = spellConfig.spellType;
                _spellPools.Add(spellType, new SpellPool(prefab));
            }
        }
        
        public void Dispose()
        {
            _onSpellSelectedSubsctiption?.Dispose();
        }

        private void ApplyVfxToCurrentItem(SpellType spellType)
        {
            var vfx = _spellPools[spellType].Rent();
            Vector3 newPosition = GameObject
                .FindObjectOfType<CurrentWeaponSpawnPoint>().transform.position;
            newPosition.x += 3f;
            vfx.transform.position = newPosition;

        }
    }
}