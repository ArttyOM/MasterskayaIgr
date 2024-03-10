using System;
using Code.DebugTools.Logger;
using Code.Main;
using Code.Spells;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

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
        private Slider _hpVisual;

        private IDisposable _onTriggerEnterSubscription;
        private IDisposable _onCollisionStaySubscription;
        private IDisposable _onCollisionExitSubsruption;
        
        private IObserver<(CommonEnemy, SpellExplosion)> _onExplosionEnter;
        private IObserver<CommonEnemy> _onEnemyDead;

        public ObservableCollision2DTrigger GetObservableCollision2DTrigger { get; private set;}
        public ObservableTrigger2DTrigger GetObservableTrigger2DTrigger { get; private set; }
        public EnemyType GetEnemyType => _enemyType;

        public float GetBaseSpeed => _baseSpeed;

        public void Init(IObserver<(CommonEnemy, SpellExplosion)> onExplosionEnter, IObserver<CommonEnemy> onEnemyDead,EnemyStats config)
        {
            _onExplosionEnter = onExplosionEnter;
            _onEnemyDead = onEnemyDead;
            if (_rigidbody2D is null) _rigidbody2D = FindKinematicRigidbody();
            GetObservableCollision2DTrigger = GetComponentInChildren<ObservableCollision2DTrigger>();
            GetObservableTrigger2DTrigger = GetComponentInChildren<ObservableTrigger2DTrigger>();
            _hpVisual = GetComponentInChildren<Slider>();
            _hpVisual.value = 1f;
            
            _baseSpeed = config.moveSpeed;
            currentSpeed = _baseSpeed;
            _maxHP = config.hitPoints;
            _currentHP = _maxHP;

            _onTriggerEnterSubscription = GetObservableTrigger2DTrigger.OnTriggerEnter2DAsObservable()
                .Subscribe(trigger =>
                {
                    var explosion = trigger.GetComponentInChildren<SpellColliderProvider>().GetComponentInParent<SpellExplosion>();
                    if (explosion is not null)
                    {
                        _onExplosionEnter.OnNext(new (this,explosion));
                    }
                    ">>OnTriggerEnter".Colored(Color.red).Log();
                });

            _onCollisionStaySubscription = GetObservableCollision2DTrigger.OnCollisionStay2DAsObservable()
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

        public bool _dead;
        public void GetHit(float damage)
        {
            var name = this.name;
            ServiceLocator.Instance.DamageNumbers.Spawn(Mathf.CeilToInt(damage), this.transform.position);
            $">>GetHit {name} got {damage} damage".Colored(Color.cyan).Log();

            _currentHP -= damage;

            _hpVisual.value = _currentHP / _maxHP;
            
            if (_currentHP <= 0 && !_dead)
            {
                _dead = true;
                Destroy(this.gameObject);
                _onEnemyDead.OnNext(this);
            }
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
            _onCollisionStaySubscription?.Dispose();
        }
    }
}