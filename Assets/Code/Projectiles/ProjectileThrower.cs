using System;
using System.Collections.Generic;
using Code.DebugTools.Logger;
using Code.Spells;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace Code.Projectiles
{
    public class ProjectileThrower : IDisposable
    {
        public ProjectileThrower(IObservable<(Vector2Int, Vector3)> eventsProjectileDestinationSelected, IObserver<ExplosionData> onExplosion,
            IReadOnlyDictionary<ProjectileType, WeaponPool> weaponPools,
            IReadOnlyDictionary<SpellType, SpellPool> spellPools)
        {
            _onExplosion = onExplosion;
            _weaponPools = weaponPools;
            _spellPools = spellPools;
            _projectilePool = new();

            _spawnPoint = GameObject.FindObjectOfType<CurrentWeaponSpawnPoint>();
            _onFireSubsctription = eventsProjectileDestinationSelected
                .Throttle(TimeSpan.FromMilliseconds(300f))
                .Subscribe(x => BuildProjectileThenThrow(x));
        }

        private readonly IObserver<ExplosionData> _onExplosion;
        private readonly IReadOnlyDictionary<ProjectileType, WeaponPool> _weaponPools;
        private readonly IReadOnlyDictionary<SpellType, SpellPool> _spellPools;
        private readonly IDisposable _onFireSubsctription;
        private readonly CurrentWeaponSpawnPoint _spawnPoint;
        private readonly ProjectilePool _projectilePool;

        public void Dispose()
        {
            _onFireSubsctription?.Dispose();
        }

        private void BuildProjectileThenThrow((Vector2Int gridCoords, Vector3 worldPosition) destinationPoint)
        {
            var weapon = _spawnPoint.GetComponentInChildren<Weapon>();
            if (weapon is null) return;
            var spell = _spawnPoint.GetComponentInChildren<Spell>();
            //if (spell is null) return;

            var currentProjectile = _projectilePool.Rent();
            var currentProjectileTransform = currentProjectile.transform;
            currentProjectileTransform.position = _spawnPoint.transform.position;
            weapon.transform.SetParent(currentProjectileTransform, true);
            spell?.transform.SetParent(currentProjectile.transform, true);

            $"destination worlposition = {destinationPoint.worldPosition}".Colored(Color.cyan).Log();
            currentProjectile.transform.DOMove(destinationPoint.worldPosition, 0.3f).OnComplete(() =>
            {
                "Выстрел совершен".Colored(Color.cyan).Log();
                _onExplosion.OnNext(new ExplosionData(weapon.GetProjectileType, spell.GetSpellType, destinationPoint.worldPosition,destinationPoint.gridCoords));
                _weaponPools[weapon.GetProjectileType].Return(weapon);
                _spellPools[spell.GetSpellType].Return(spell);
            });
        }
        
    }
}