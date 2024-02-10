using UnityEngine;

namespace Code.Spells
{
    public class SpellExplosion:MonoBehaviour
    {
        private SpellColliderProvider _colliderProvider;

        public SpellColliderProvider GetColliderProvider => _colliderProvider;
        
        private void Awake()
        {
            _colliderProvider = GetComponentInChildren<SpellColliderProvider>();
        }
        
    }
}