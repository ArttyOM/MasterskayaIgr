using System;
using System.Collections.Generic;
using UnityEngine;
using Code.DebugTools.Logger;
using MyBox;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Enemies
{
    public class CommonEnemyMover : IDisposable
    {
        public CommonEnemyMover(CommonEnemy prefab, float moveSpeed, IObservable<int> eventsSessionStart)
        {
            _enemyPool = new CommonEnemyPool(prefab);
            _baseMoveSpeed = moveSpeed;
            _currentMoveSpeed = _baseMoveSpeed;
            
            var alreadyExistingEnemies = Object.FindObjectsByType<CommonEnemy>(FindObjectsSortMode.None);
            foreach (var enemy in alreadyExistingEnemies)
            {
                enemy.Speed = _baseMoveSpeed; // Установка базовой скорости для каждого врага
                _enemies.Add(enemy);

                var onCollision = enemy.GetComponentInChildren<ObservableCollision2DTrigger>();
                onCollision.OnCollisionEnter2DAsObservable().Subscribe(x =>
                {
                    enemy.Speed = 0; // Обнуление скорости при столкновении
                });
            }
            _startSessionSubscription = eventsSessionStart.Subscribe(x => Activate());
            Observable.EveryFixedUpdate().Subscribe(x => Move());
        }

        private IDisposable _startSessionSubscription;
        private CommonEnemyPool _enemyPool;
        private List<CommonEnemy> _enemies = new List<CommonEnemy>();
        private float _baseMoveSpeed;
        private float _currentMoveSpeed;
        private bool _is_active = false;
        private void Move()
        {
            if (!_is_active) return;

            foreach (var enemy in _enemies)
            {
                var rb = enemy.GetKinematicRigidbody;
                if (rb != null)
                {
                    rb.velocity = new Vector2(-enemy.Speed, 0); // Использование индивидуальной скорости
                }
            }
        }

        private void Activate()
        {
            _is_active = true;
            // Восстановление текущей скорости до базовой для всех врагов при активации
            foreach (var enemy in _enemies)
            {
                enemy.Speed = _baseMoveSpeed;
            }
        }

        public void Dispose()
        {
            _enemyPool?.Dispose();
            _startSessionSubscription?.Dispose();
        }
    }
}
