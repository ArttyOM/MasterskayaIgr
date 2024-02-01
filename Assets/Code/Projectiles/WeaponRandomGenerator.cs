using System;
using System.Collections.Generic;
using Code.DebugTools.Logger;
using DG.Tweening;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Code.Projectiles
{
    public class WeaponRandomGenerator: IDisposable
    {
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
        
        private readonly Dictionary<ProjectileType, WeaponPool> _weaponPools = new();
        private readonly WeaponSpawnChanceConfig _weaponSpawnChanceConfig;

        private readonly IDisposable _onNextWeaponSubsctiption;

        private readonly NextWeaponSpawnPoint _nextWeaponSpawnPoint;
        private readonly CurrentWeaponSpawnPoint _currentWeaponSpawnPoint;

        private Weapon _currentWeapon;
        private Weapon _nextWeapon;
        
        
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
            MoveNextWeaponToCurrentPosition();

            ProjectileType typeOfWeapon = GenarateRandomNextWeapon();
            _nextWeapon = _weaponPools[typeOfWeapon].Rent();
            
            _nextWeapon.transform.position = _nextWeaponSpawnPoint.transform.position;

        }

        private ProjectileType GenarateRandomNextWeapon()
        {
            int sum = 0;

            List<(int, ProjectileType)> spawnChances = new();
            
            foreach (var weaponPriorityPair in _weaponSpawnChanceConfig.weaponPriorityPairs)
            {
                sum += weaponPriorityPair.priority;
                if (weaponPriorityPair.priority > 0)
                {
                    spawnChances.Add((sum, weaponPriorityPair.weaponPrefab.GetProjectileType));
                }
            }

            int random = Random.Range(0, sum);
            foreach (var spawnChance in spawnChances)
            {
                if (spawnChance.Item1 > random) return spawnChance.Item2;
            }
            
            "В конфиге все приоритеты спауна нулевые".Colored(Color.red).LogError();
            return ProjectileType.Beaver;
        }

        private void MoveNextWeaponToCurrentPosition()
        {
            if (_nextWeapon != null)
            {
                if (_currentWeapon != null)
                {
                    _weaponPools[_currentWeapon.GetProjectileType].Return(_currentWeapon);
                }
                _currentWeapon = _nextWeapon;
                _currentWeapon.transform.DOMove(_currentWeaponSpawnPoint.transform.position, 0.4f);
            }
        }
    }
}