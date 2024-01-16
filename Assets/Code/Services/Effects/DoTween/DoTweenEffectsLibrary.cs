using DG.Tweening;
using UnityEngine;

namespace Code.Services.Effects.DoTween
{
    public static class DoTweenEffectsLibrary
    {
        public static Tweener DoSimpleBounce(this Transform transform, float duration = 0.3f, float target = 0.75f,
            Ease ease = Ease.OutElastic)
        {
            return transform.DOScale(Vector3.one * target, duration)
                .From()
                .SetEase(ease);
        }

        public static Tweener DoSimpleBounce(this Transform transform, Vector3 target, Ease ease = Ease.OutElastic,
            float duration = 0.3f)
        {
            return transform.DOScale(target, duration)
                .From()
                .SetEase(ease);
        }

        public static Tweener DoPulse(this Transform transform, float duration = 1f, float target = 1.5f,
            Ease ease = Ease.Linear)
        {
            return transform.DOScale(Vector3.one * target, duration)
                .SetEase(ease)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}