using System;
using UnityEngine;

namespace Code.Spells
{
    [RequireComponent(typeof(Collider2D))]
    public class SpellColliderProvider : MonoBehaviour
    {
        private Collider2D _collider2D;

        public Collider2D GetCollider2D => _collider2D;
        
        private void Awake()
        {
            _collider2D = GetComponent<Collider2D>();
        }
    }
}