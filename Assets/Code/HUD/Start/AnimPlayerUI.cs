using System;
using UnityEngine;

namespace Code.HUD.Start
{
    public class AnimPlayerUI : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private AnimationClip _animation;
        

        private void OnEnable()
        {
            _animator.enabled = true;
            _animator.Play(_animation.name);
        }

        private void OnDisable()
        {
            _animator.enabled = false;
        }
    }
}