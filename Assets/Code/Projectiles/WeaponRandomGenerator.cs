using System;
using System.Collections.Generic;
using Code.DebugTools.Logger;
using DG.Tweening;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Projectiles
{
    public class WeaponRandomGenerator: IDisposable
    {
        private readonly Dictionary<ProjectileType, WeaponPool> _weaponPools = new();
        private readonly WeaponSpawnChanceConfig _weaponSpawnChanceConfig;

        private readonly IDisposable _onNextWeaponSubsctiption;

        private readonly NextWeaponSpawnPoint _nextWeaponSpawnPoint;
        private readonly CurrentWeaponSpawnPoint _currentWeaponSpawnPoint;

        private Weapon _currentWeapon;
        private Weapon _nextWeapon;
        
        public WeaponRandomGenerator(WeaponSpawnChanceConfig weaponSpawnChanceConfig)
        {
            _nextWeaponSpawnPoint = Object.FindObjectOfType<NextWeaponSpawnPoint>();
            _currentWeaponSpawnPoint = Object.FindObjectOfType<CurrentWeaponSpawnPoint>();
            
            _weaponSpawnChanceConfig = weaponSpawnChanceConfig;
            CreatePools(weaponSpawnChanceConfig);

            _onNextWeaponSubsctiption = Observable.EveryUpdate()
                .Where(_ => Input.GetKeyUp(KeyCode.Space))
                .Subscribe(_ => GenerateWeapon());
        }
        
        
        public void Dispose()
        {
            _onNextWeaponSubsctiption?.Dispose();
        }

        private void CreatePools(WeaponSpawnChanceConfig weaponSpawnChanceConfig)
        {
            Weapon prefab;
            ProjectileType projectileType;
            foreach (var weaponPriorityPair in  weaponSpawnChanceConfig.weaponPriorityPairs)
            {
                prefab = weaponPriorityPair.weaponPrefab;
                projectileType = prefab.GetProjectileType;
                _weaponPools.Add(projectileType, new WeaponPool(prefab));
            }
        }

        private void GenerateWeapon()
        {
            ">>GenerateWeapon".Colored(Color.green);
            if (_nextWeapon != null)
            {
                _currentWeapon = _nextWeapon;
                _currentWeapon.transform.DOMove(_currentWeaponSpawnPoint.transform.position, 0.4f);
            }

            _nextWeapon = _weaponPools[ProjectileType.Beaver].Rent();
            _nextWeapon.transform.position = _nextWeaponSpawnPoint.transform.position;

        }
    }
}