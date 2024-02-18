using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.WebCam;
using Object = UnityEngine.Object;

namespace Code.Enemies
{
    public class Enemies: IDisposable
    {
        public Enemies(EnemiesConfig enemiesConfig)
        {
            _enemiesConfig = enemiesConfig;

            foreach (var enemyConfigEnemyConfig in _enemiesConfig.enemyConfigs)
            {
                _enemyPools.Add(enemyConfigEnemyConfig.enemyType, new CommonEnemyPool(enemyConfigEnemyConfig.prefab));
                _enemyStatsMap.Add(enemyConfigEnemyConfig.enemyType, enemyConfigEnemyConfig.enemyStats);
            }
            var existingEnemies = Object.FindObjectsByType<CommonEnemy>(FindObjectsSortMode.None);
            foreach (var enemy in existingEnemies)
            {
                if (!_aliveEnemies.ContainsKey(enemy.GetEnemyType))
                {
                    _aliveEnemies.Add(enemy.GetEnemyType, new List<CommonEnemy>(){enemy});
                }
                else _aliveEnemies[enemy.GetEnemyType].Add(enemy);
            }
        }

        private readonly EnemiesConfig _enemiesConfig;
        private readonly Dictionary<EnemyType, CommonEnemyPool> _enemyPools = new();
        private readonly Dictionary<EnemyType, List<CommonEnemy>> _aliveEnemies = new();
        private readonly Dictionary<EnemyType, EnemyStats> _enemyStatsMap = new();

        public IReadOnlyDictionary<EnemyType, CommonEnemyPool> GetEnemyPools => _enemyPools;
        public IReadOnlyDictionary<EnemyType, EnemyStats> GetEnemyStats => _enemyStatsMap;

        public IReadOnlyList<CommonEnemy> GetAliveEnemies
        {
            get
            {
                var result = new List<CommonEnemy>();
                foreach (var enemiesGroupPair in _aliveEnemies)
                {
                    result.AddRange(enemiesGroupPair.Value);
                }
                return result;
            }
        }


        public void Dispose()
        {
        }
    }
}