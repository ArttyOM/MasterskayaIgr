using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Code.DebugTools.Logger;
using Code.Spells;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Code.Projectiles
{
    public class WeaponRandomGenerator: IDisposable
    {
        public const float Duration = 0.04f;
        
        public WeaponRandomGenerator(WeaponSpawnChanceConfig weaponSpawnChanceConfig)
        {
            _nextWeaponSpawnPoint = Object.FindObjectOfType<NextWeaponSpawnPoint>();
            _currentWeaponSpawnPoint = Object.FindObjectOfType<CurrentWeaponSpawnPoint>();
            
            _weaponSpawnChanceConfig = weaponSpawnChanceConfig;
            CreatePools(weaponSpawnChanceConfig);
        }
        
        private readonly Dictionary<ProjectileType, WeaponPool> _weaponPools = new();
        private readonly WeaponSpawnChanceConfig _weaponSpawnChanceConfig;

        private IDisposable _onNextWeaponSubsctiption;

        private readonly NextWeaponSpawnPoint _nextWeaponSpawnPoint;
        private readonly CurrentWeaponSpawnPoint _currentWeaponSpawnPoint;

        private Weapon _loadedWeapon;
        private Weapon _nextWeapon;


        public IReadOnlyDictionary<ProjectileType, WeaponPool> GetWeaponPools => _weaponPools;
        
        public void Dispose()
        {
            _onNextWeaponSubsctiption?.Dispose();
        }
        
        public void GenerateWeapon(out Weapon loadedWeapon, out Weapon nextWeapon)
        {
            ">>GenerateWeapon".Colored(Color.green).Log();
            
            ProjectileType typeOfWeapon = GenarateRandomNextWeapon();
            _loadedWeapon = _nextWeapon;
            _nextWeapon = _weaponPools[typeOfWeapon].Rent();
            _nextWeapon.transform.position = _nextWeaponSpawnPoint.transform.position;

            nextWeapon = _nextWeapon;
            loadedWeapon = _loadedWeapon;
        }

        public void SendWeaponToLoadedPositionInstantly(Weapon weapon)
        {
            var transform = _currentWeaponSpawnPoint.transform;
            weapon.transform.position = transform.position;
            weapon.transform.SetParent(transform);
        }
        
        public async void SendWeaponToLoadedPosition(Weapon weapon, CancellationToken cancellationToken)
        {
            await weapon.transform.DOMove(_currentWeaponSpawnPoint.transform.position,
                WeaponRandomGenerator.Duration).WithCancellation(cancellationToken);
            weapon.transform.SetParent(_currentWeaponSpawnPoint.transform);
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

        
        private async Task MoveWeaponToPreparedPosition([NotNull] Weapon weapon)
        {
            //_weaponPools[_currentWeapon.GetProjectileType].Return(_currentWeapon);
            weapon.transform.SetParent(_currentWeaponSpawnPoint.transform);
            weapon.transform.DOMove(_currentWeaponSpawnPoint.transform.position, Duration);
        }
    }
}