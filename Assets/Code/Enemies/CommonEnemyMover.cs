using System;
using System.Collections.Generic;
using UnityEngine;
using Code.DebugTools.Logger;
using MyBox;
using UniRx;
using UniRx.Triggers;
<<<<<<< HEAD
using UnityEngine;
using Object = UnityEngine.Object;
=======
>>>>>>> origin/iusup-stenka

namespace Code.Enemies
{
    public class CommonEnemyMover : IDisposable
    {
        public CommonEnemyMover(CommonEnemy prefab, float moveSpeed, IObservable<int> eventsSessionStart)
        {
            _enemyPool = new CommonEnemyPool(prefab);
            _baseMoveSpeed = moveSpeed;
            _currentMoveSpeed = _baseMoveSpeed;

<<<<<<< HEAD
            var alreadyExistingEnemies = Object.FindObjectsByType<CommonEnemy>(FindObjectsSortMode.None);
            foreach (var commonEnemy in alreadyExistingEnemies)
            {
                _enemiesRigidbody2Ds.Add(commonEnemy.GetKinematicRigidbody);
                var onCollision = commonEnemy.GetComponentInChildren<ObservableCollision2DTrigger>();
=======
            var alreadyExistingEnemies = GameObject.FindObjectsOfType<CommonEnemy>();
            foreach (var enemy in alreadyExistingEnemies)
            {
                enemy.Speed = _baseMoveSpeed; // Установка базовой скорости для каждого врага
                _enemies.Add(enemy);

                var onCollision = enemy.GetComponentInChildren<ObservableCollision2DTrigger>();
>>>>>>> origin/iusup-stenka
                onCollision.OnCollisionEnter2DAsObservable().Subscribe(x =>
                {
                    enemy.Speed = 0; // Обнуление скорости при столкновении
                });
            }

<<<<<<< HEAD
            _moveSpeed = moveSpeed;

            _startSessionSubscription = eventsSessionStart.Subscribe(x => Activate());
            Observable.EveryFixedUpdate().Subscribe(x => Move());
        }

        private IDisposable _startSessionSubscription;

        private CommonEnemyPool _enemyPool;

        private List<Rigidbody2D> _enemiesRigidbody2Ds = new();
        private float _moveSpeed;
        private bool _is_active = false;

        private void Move()
        {
            if (_is_active)
                foreach (var rigidbody2D in _enemiesRigidbody2Ds)
                    rigidbody2D.velocity = new Vector2(-_moveSpeed, 0f);
=======
            _startSessionSubscription = eventsSessionStart.Subscribe(_ => Activate());
            Observable.EveryFixedUpdate().Subscribe(_ => Move());
        }

        private CommonEnemyPool _enemyPool;
        private List<CommonEnemy> _enemies = new List<CommonEnemy>();
        private float _baseMoveSpeed;
        private float _currentMoveSpeed;
        private IDisposable _startSessionSubscription;
        private bool _is_active = false;
        private void Move()
        {
            if (!_is_active) return;

            foreach (var enemy in _enemies)
            {
                var rb = enemy.GetKinematicRigidbody();
                if (rb != null)
                {
                    rb.velocity = new Vector2(-enemy.Speed, 0); // Использование индивидуальной скорости
                }
            }
>>>>>>> origin/iusup-stenka
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
