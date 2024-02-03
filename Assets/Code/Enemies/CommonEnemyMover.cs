using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Code.Enemies
{
    public class CommonEnemyMover: IDisposable
    {
        public CommonEnemyMover(CommonEnemy prefab, float moveSpeed)
        {
            _enemyPool = new CommonEnemyPool(prefab);

            var alreadyExistingEnemies = GameObject.FindObjectsByType<CommonEnemy>(FindObjectsSortMode.None);
            foreach (var commonEnemy in alreadyExistingEnemies)
            {
                _enemiesRigidbody2Ds.Add(commonEnemy.GetKinematicRigidbody());
            }

            _moveSpeed = moveSpeed;
            Observable.EveryFixedUpdate().Subscribe(x => Move());
        }

        private CommonEnemyPool _enemyPool;

        private List<Rigidbody2D> _enemiesRigidbody2Ds = new ();
        private float _moveSpeed;
        private bool _is_active = false;
        
        private void Move()
        {
            if (_is_active)
            {
                foreach (var rigidbody2D in _enemiesRigidbody2Ds)
                {
                    rigidbody2D.velocity = new Vector2(-_moveSpeed, 0f);
                }
            }
        }

        public void Activate()
        {
            _is_active = true;
        }

        public void Dispose()
        {
            _enemyPool.Dispose();
        }
    }
}