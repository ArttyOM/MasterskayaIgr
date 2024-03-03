using System;
using Code.Main;
using DG.Tweening;
using MyBox;
using UnityEngine;

namespace Code.InGameRewards
{
    public class DropCoin : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Ease _spreadEase = Ease.InOutQuad;
        [SerializeField] private Ease _shakeEase = Ease.Linear;
        [SerializeField] private float _spreadRange = 200;
        
        [SerializeField] private Ease _flyToTargetEase = Ease.InExpo;
        [SerializeField] private float _spreadDuration = .2f;
        [SerializeField] private float _shakeDuration = .2f;
        [SerializeField] private float _shakeStrength = .2f;
        [SerializeField] private float _delayDuration = .2f;
        [SerializeField] private float _flyToTargetDuration = .5f;
        [SerializeField][Range(0,1)] private float _randomizeDelay;
        [SerializeField] private Vector2 _randomRange;
        
        
        
        private Sequence _sequence;

        public event Action<DropCoin> OnComplete;
        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public void Drop(Vector2 localPoint, Vector2 destination)
        {
            _rectTransform.position = localPoint;
            // _sequence?.Kill(true);
            _sequence = DOTween.Sequence(this);
            _sequence.Append(_rectTransform.DOLocalMove(
                    _rectTransform.localPosition + (Vector3)UnityEngine.Random.insideUnitCircle * _spreadRange, _spreadDuration).SetEase(_spreadEase));
            _sequence.Append(_rectTransform.DOShakeScale(_shakeDuration, _shakeStrength).SetEase(_shakeEase));
            _sequence.Append(DOVirtual.DelayedCall((1-_randomizeDelay) * _delayDuration + (_randomizeDelay * UnityEngine.Random.Range(_randomRange.x, _randomRange.y)), () => { }));
            _sequence.Append(_rectTransform.DOMove(destination, _flyToTargetDuration).SetEase(_flyToTargetEase));
            _sequence.OnComplete(Complete);
        }

        private void Complete()
        {
            //@todo: Move coin change to separate service 
            ServiceLocator.Instance.Profile.GetWallet().Add(1);
            OnComplete?.Invoke(this);
        }
    }
}