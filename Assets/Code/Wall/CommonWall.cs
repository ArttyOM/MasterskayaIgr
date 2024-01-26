using Code.Enemies;
using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Code.Wall
{
    
    public class CommonWall : MonoBehaviour
    {

        private CommonEnemyMover _commonEnemyMover;
        private CompositeDisposable _disposable = new CompositeDisposable();
        public BoxCollider2D trigger;
        private float _moveSpeed = 0f;

        public void WallStopper(CommonEnemy commonEnemyPrefab)
        {
            trigger.OnTriggerEnter2DAsObservable()
                .Where(t => t.gameObject.layer == LayerMask.NameToLayer("Body"))
                .Subscribe(_ =>
                {
                    StopEnemy(commonEnemyPrefab);
                }).AddTo(_disposable);
        }

        void StopEnemy(CommonEnemy commonEnemyPrefab)
        {
            _commonEnemyMover = new CommonEnemyMover(commonEnemyPrefab, _moveSpeed);
        }
    }


}
