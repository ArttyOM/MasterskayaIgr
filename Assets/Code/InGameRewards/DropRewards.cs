using UnityEngine;
using UnityEngine.Pool;

namespace Code.InGameRewards
{
    public class DropRewards : MonoBehaviour
    {
        [SerializeField] private ObjectPool<DropCoin> _coinsPool;
        [SerializeField] private DropCoin _prefab;
        [SerializeField] private RectTransform _root;
        [SerializeField] private Camera _camera;
        private Vector2 _destination;


        private void Awake()
        {
            _coinsPool = new ObjectPool<DropCoin>(CreateCoin, OnAcquire, OnRelease, OnCoinDestroyed);
        }

        private void OnCoinDestroyed(DropCoin coin)
        {
            coin.OnComplete -= _coinsPool.Release;
        }

        private void OnRelease(DropCoin coin)
        {
            coin.Deactivate();
        }

        private void OnAcquire(DropCoin coin)
        {
            coin.Activate();
        }


        public void SetDestination(Vector2 destination)
        {
            _destination = destination;
        }

        private DropCoin CreateCoin()
        {
            var coin = Instantiate(_prefab, _root);
            coin.OnComplete += _coinsPool.Release;
            coin.Deactivate();
            return coin;
        }

        public void DropCoins(int amount, Vector2 screenSpacePoint)
        {
            if (_coinsPool == null) return;
            for (int i = 0; i < amount; i++)
            {
                var coin = _coinsPool.Get();
                coin.Drop(screenSpacePoint, _destination);
            }
        }
    }
}