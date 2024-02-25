using System;
using Code.Enemies;
using UniRx;
using UnityEngine;

namespace Code.InGameRewards
{
    public class EnemyDropService : IDisposable
    {
        private readonly IDisposable _onEnemyDead;
        private readonly EnemiesConfig _config;
        private readonly Camera _camera;
        private readonly DropRewards _rewardsService;

        public EnemyDropService(Subject<CommonEnemy> enemyDead, EnemiesConfig config, Camera camera, DropRewards rewardsService)
        {
            _config = config;
            _camera = camera;
            _rewardsService = rewardsService;
            _onEnemyDead = enemyDead.Subscribe(DropCoinsOnDeath);
        }
        private void DropCoinsOnDeath(CommonEnemy enemy)
        {
            var position = enemy.transform.position;
            var screenPoint = _camera.WorldToScreenPoint(position);
            var index = _config.enemyConfigs.FindIndex(e => enemy.GetEnemyType == e.enemyType);
            if (index < 0)
            {
                Debug.LogError("Enemy not find in config, can't drop gold");
                return;
            }

            var stats = _config.enemyConfigs[index].enemyStats;
            _rewardsService.DropCoins(stats.KillReward, screenPoint);
        }

        public void Dispose()
        {
            _onEnemyDead?.Dispose();
        }
    }
}