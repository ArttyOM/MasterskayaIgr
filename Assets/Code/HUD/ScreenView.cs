using System;
using DG.Tweening;
using UnityEngine;

namespace Code.HUD
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CanvasGroup))]
    public class ScreenView : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;
        [field: SerializeField] public ScreenType type { get; private set; }
        [SerializeField] private float _duration;
        [SerializeField] private Ease _ease;

        public void Hide(bool instantly)
        {
            _canvasGroup.DOKill(true);
            if (instantly)
            {
                gameObject.SetActive(false);
                return;
            }

            _canvasGroup.DOFade(0, _duration).SetEase(_ease).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }

        public void Show()
        {
            _canvasGroup.DOKill(true);
            _canvasGroup.alpha = 0;
            gameObject.SetActive(true);
            _canvasGroup.DOFade(1, _duration).SetEase(_ease);
        }
    }
}