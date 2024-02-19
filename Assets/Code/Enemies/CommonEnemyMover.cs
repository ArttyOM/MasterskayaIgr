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
        public CommonEnemyMover(EnemiesConfig enemiesConfig, Enemies enemies, Subject<int> eventsSessionStart)
        {
            _enemies = enemies;
            var aliveEnemies = _enemies.GetAliveEnemies;
            foreach (var enemy in aliveEnemies)
            {
                _enemiesToMove.Add(enemy);

                var onCollision = enemy.GetComponentInChildren<ObservableCollision2DTrigger>();
                // onCollision.OnCollisionEnter2DAsObservable().Subscribe(x =>
                // {
                //     enemy.CurrentSpeed = 0; // Обнуление скорости при столкновении
                // });
            }
            _movementSubscription = Observable.EveryFixedUpdate()
                .SkipUntil(eventsSessionStart)
                .Subscribe(x => Move());
        }
        
        private readonly IDisposable _movementSubscription;
        private List<CommonEnemy> _enemiesToMove = new();
        private float _baseMoveSpeed;
        private float _currentMoveSpeed;
        private readonly Enemies _enemies;

        private void Move()
        {
            foreach (var enemy in _enemiesToMove)
            {
                var rb = enemy.GetKinematicRigidbody;
                if (rb != null)
                {
                    rb.velocity = new Vector2(-enemy.currentSpeed, 0); // Использование индивидуальной скорости
                }
            }
        }
        

        public void Dispose()
        {
            _movementSubscription?.Dispose();
        }
    }
}
