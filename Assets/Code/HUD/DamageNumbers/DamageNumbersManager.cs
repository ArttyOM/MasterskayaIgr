using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

namespace Code.HUD.DamageNumbers
{
    public class DamageNumbersManager : MonoBehaviour
    {
        [FormerlySerializedAs("_coinsPool")] [SerializeField] private ObjectPool<DamageNumber> _dmPool;
        [SerializeField] private DamageNumber _prefab;
        [SerializeField] private RectTransform _root;
        [SerializeField] private Camera _camera;
        private Vector2 _destination;


        private void Awake()
        {
            _dmPool = new ObjectPool<DamageNumber>(CreateNumber, OnAcquire, OnRelease, OnCoinDestroyed);
        }

        private void OnCoinDestroyed(DamageNumber dm)
        {
            dm.OnComplete -= _dmPool.Release;
        }

        private void OnRelease(DamageNumber dm)
        {
            dm.Deactivate();
        }

        private void OnAcquire(DamageNumber dm)
        {
            dm.Activate();
        }
        private DamageNumber CreateNumber()
        {
            var coin = Instantiate(_prefab, _root);
            coin.OnComplete += _dmPool.Release;
            coin.Deactivate();
            return coin;
        }

        public void Spawn(int amount, Vector3 worldPosition)
        {
            if (_dmPool == null) return;
            for (int i = 0; i < amount; i++)
            {
                var dm = _dmPool.Get();
                dm.Drop(amount, _camera.WorldToScreenPoint(worldPosition));
            }
        }
    }
}