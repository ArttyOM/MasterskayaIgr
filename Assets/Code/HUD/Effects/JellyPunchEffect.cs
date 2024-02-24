using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Code.HUD.Effects
{
    public class JellyPunchEffect : Effect
    {
        [SerializeField] private Image _image;
        [SerializeField] private float _scale;
        [SerializeField] private float _duration;
        [SerializeField] private Ease _ease;
        
        public override void Play()
        {
            _image.transform.DOKill(true);
            _image.transform.DOPunchScale(Vector3.one * _scale, _duration).SetEase(_ease);
        }
    }
}