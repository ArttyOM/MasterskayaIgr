using System;
using Code.DebugTools.Logger;
using Code.Spells;
using UniRx;
using UniRx.Diagnostics;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Enemies
{
    public class CommonEnemy:MonoBehaviour
    {
        [SerializeField] private EnemyType _enemyType;
        private Rigidbody2D _rigidbody2D;
        private float _currentHP;
        private float _maxHP = 20f;
        private float _baseSpeed;
        public float currentSpeed;

        private IDisposable _onTriggerEnterSubscription;
        private IDisposable _onCollisionEnterSubscription;
        private IDisposable _onCollisionExitSubsruption;
        
        private IObserver<(CommonEnemy, SpellExplosion)> _onExplosionEnter;
        
        public ObservableCollision2DTrigger GetObservableCollision2DTrigger { get; private set;}
        public ObservableTrigger2DTrigger GetObservableTrigger2DTrigger { get; private set; }
        public EnemyType GetEnemyType => _enemyType;

        public void Init(IObserver<(CommonEnemy, SpellExplosion)> onExplosionEnter, EnemyStats config)
        {
            _onExplosionEnter = onExplosionEnter;
            
            if (_rigidbody2D is null) _rigidbody2D = FindKinematicRigidbody();
            GetObservableCollision2DTrigger = GetComponentInChildren<ObservableCollision2DTrigger>();
            GetObservableTrigger2DTrigger = GetComponentInChildren<ObservableTrigger2DTrigger>();

            _baseSpeed = config.moveSpeed;
            currentSpeed = _baseSpeed;
            _maxHP = config.hitPoints;
            _currentHP = _maxHP;

            _onTriggerEnterSubscription = GetObservableTrigger2DTrigger.OnTriggerEnter2DAsObservable()
                .Subscribe(trigger =>
                {
                    var explosion = trigger.GetComponent<SpellColliderProvider>().GetComponentInParent<SpellExplosion>();
                    if (explosion is not null)
                    {
                        _onExplosionEnter.OnNext(new (this,explosion));
                    }
                    "OnTriggerEnter>>".Colored(Color.red).Log();
                });

            _onCollisionEnterSubscription = GetObservableCollision2DTrigger.OnCollisionEnter2DAsObservable()
                .Subscribe(_ =>
                {
                    currentSpeed = 0;
                });
            _onCollisionExitSubsruption = GetObservableCollision2DTrigger.OnCollisionExit2DAsObservable()
                .Subscribe(_ =>
                {
                    currentSpeed = _baseSpeed;
                });
        }
        
        public Rigidbody2D GetKinematicRigidbody
        {
            get
            {
                if (_rigidbody2D is null) _rigidbody2D = FindKinematicRigidbody();
                return _rigidbody2D;
            }
        }

        public void GetHit(float damage)
        {
            var name = this.name;
            $">>GetHit {name} got {damage} damage".Colored(Color.cyan).Log();

            _currentHP -= damage;
            if (_currentHP<=0) Destroy(this.gameObject);
        }
        
        
        private Rigidbody2D FindKinematicRigidbody()
        {
            var allRigidbodies = GetComponentsInChildren<Rigidbody2D>();
            if (allRigidbodies == null || allRigidbodies.Length == 0)
            {
                "GetComponentsInChildren<Rigidbody2D> вернул пустой список".Colored(Color.red).LogError();
                return null;
            }

            foreach (var rigidbody2D in allRigidbodies)
                if (rigidbody2D.isKinematic)
                    return rigidbody2D;
            {
                "Среди rigidbody2d в дочерних объектах ни одного кинематичного".Colored(Color.red).LogError();
                return null;
            }
        }
        

        private void OnDestroy()
        {
            _onTriggerEnterSubscription?.Dispose();
            _onCollisionExitSubsruption?.Dispose();
            _onCollisionEnterSubscription?.Dispose();
        }
    }
}