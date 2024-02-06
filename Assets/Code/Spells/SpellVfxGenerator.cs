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
        public SpellVfxGenerator(SpellsConfig spellsConfig, IObservable<SpellType> onSpellSelected,
            IObservable<int> eventsSessionStart, 
            IObservable<(Vector2Int, Vector3)> eventsProjectileDestinationSelected)
        {
            _currentWeaponSpawnPoint = GameObject
                .FindObjectOfType<CurrentWeaponSpawnPoint>();
            CreatePools(spellsConfig);
            
            _onSpellSelectedSubsctiption = onSpellSelected.SkipUntil(eventsSessionStart)
                .Subscribe(ApplyVfxToCurrentItem);
        }

        private IDisposable _onSpellSelectedSubsctiption;

        private readonly Dictionary<SpellType, SpellPool> _spellPools = new();

        private readonly CurrentWeaponSpawnPoint _currentWeaponSpawnPoint;

        public IReadOnlyDictionary<SpellType, SpellPool> GetSpellPools => _spellPools;

        public void Dispose()
        {
            _onSpellSelectedSubsctiption?.Dispose();
        }
        
        private void CreatePools(SpellsConfig spellsConfig)
        {
            Spell prefab;
            SpellType spellType;
            foreach (var spellConfig in  spellsConfig.spellConfigs)
            {
                prefab = spellConfig.spellPreparationVfxPrefab;
                spellType = spellConfig.spellType;
                _spellPools.Add(spellType, new SpellPool(prefab));
            }
        }

        private void ApplyVfxToCurrentItem(SpellType spellType)
        {
            var vfx = _spellPools[spellType].Rent();

            var spawnPointTransform = _currentWeaponSpawnPoint.transform;
            Vector3 newPosition = spawnPointTransform.position;
            newPosition.x += 3f;
            vfx.transform.position = newPosition;
            vfx.transform.SetParent(spawnPointTransform);
            
        }
    }
}