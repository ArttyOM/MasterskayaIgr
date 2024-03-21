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
            if (_spriteRenderer == null) return;
            _spriteRenderer.enabled = false;
        }

        public void ShowOnPosition(Vector3 position)
        {
            _thisTransform.position = position;
            _spriteRenderer.enabled = true;
            
        }

        public void MoveToPosition(Vector3 position)
        {
            _thisTransform.DOMove(position,0.3f);
        }


        private void Awake()
        {
            _thisTransform = this.transform;
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
}