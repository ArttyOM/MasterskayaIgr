using System;
using DG.Tweening;
using UnityEngine;

namespace Code.Projectiles
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class AimCursor:MonoBehaviour
    {
        private Transform _thisTransform;
        private SpriteRenderer _spriteRenderer;
        

        public void Hide()
        {
            _spriteRenderer.enabled = false;
        }

        public void ShowOnPosition(Vector3 position)
        {
            if (_spriteRenderer.enabled)
            {
                _thisTransform.DOMove(position,0.3f);
            }
            else
            {
                _thisTransform.position = position;
            }
            _spriteRenderer.enabled = true;
            
        }


        private void Awake()
        {
            _thisTransform = this.transform;
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
}