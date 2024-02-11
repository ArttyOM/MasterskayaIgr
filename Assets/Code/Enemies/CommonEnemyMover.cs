using System;
using System.Collections.Generic;
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

            var alreadyExistingEnemies = Object.FindObjectsByType<CommonEnemy>(FindObjectsSortMode.None);
            foreach (var commonEnemy in alreadyExistingEnemies)
            {
                _enemiesRigidbody2Ds.Add(commonEnemy.GetKinematicRigidbody);
                var onCollision = commonEnemy.GetComponentInChildren<ObservableCollision2DTrigger>();
                onCollision.OnCollisionEnter2DAsObservable().Subscribe(x =>
                {
                    $"OnCollision {x.collider.name}".Colored(Colors.aqua).Log();
                });
            }

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
        }

        private void Activate()
        {
            _is_active = true;
        }

        public void Dispose()
        {
            _enemyPool?.Dispose();
            _startSessionSubscription?.Dispose();
        }
    }
}